using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class ApiManager : MonoBehaviour
{
    private const string BASE_API_URL = "https://gameapi.gd-alt.ru/api/";

    private string _accessToken;
    private int _userId;
    private string _userName;
    private string _userRole;

    [Serializable]
    public class RegisterRequestData
    {
        public int level_type_id;
        public int role_id;
        public int specialty_id;
        public string name;
        public string login;
        public string password;
    }

    [Serializable]
    public class RegisterResponseData
    {
        public string message;
        public int user_id;
    }

    [Serializable]
    public class LoginResponseData
    {
        public string token_type;
        public int user_id;
        public string user_name;
        public string role;
        public string access_token;
    }

    [Serializable]
    public class ProgressRequestData
    {
        public int object_id;
        public int scores;
    }

    [Serializable]
    public class ProgressResponseData
    {
        public int id;
        public int user_id;
        public int object_id;
        public int scores;
    }

    [Serializable]
    public class AccountDeleteResponse
    {
        public string status;
        public string message;
        public int id;
        public string name;
        public int deleted_progress_records;
    }

    [Serializable]
    public class ApiErrorResponse
    {
        public string detail;
    }

    // Регистрация нового пользователя
    public IEnumerator Register(int levelTypeId, int roleId, int specialtyId,
                              string name, string login, string password,
                              Action<int> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "auth/register";

        RegisterRequestData requestData = new RegisterRequestData
        {
            level_type_id = levelTypeId,
            role_id = roleId,
            specialty_id = specialtyId,
            name = name,
            login = login,
            password = password
        };

        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                RegisterResponseData response = JsonUtility.FromJson<RegisterResponseData>(request.downloadHandler.text);
                if (response.message == "User created successfully")
                {
                    onSuccess?.Invoke(response.user_id);
                }
                else
                {
                    onFailure?.Invoke("Registration failed: " + response.message);
                }
            }
            else
            {
                onFailure?.Invoke(GetErrorMessage(request));
            }
        }
    }

    // Авторизация пользователя
    public IEnumerator Login(string username, string password,
                           Action<LoginResponseData> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "auth/login";

        WWWForm form = new WWWForm();
        form.AddField("grant_type", "password");
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("scope", "");
        form.AddField("client_id", "unity_client");
        form.AddField("client_secret", "secret");

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                LoginResponseData response = JsonUtility.FromJson<LoginResponseData>(request.downloadHandler.text);
                _accessToken = response.access_token;
                _userId = response.user_id;
                _userName = response.user_name;
                _userRole = response.role;
                onSuccess?.Invoke(response);
            }
            else
            {
                onFailure?.Invoke(GetErrorMessage(request));
            }
        }
    }

    // Получение прогресса пользователя
    public IEnumerator GetUserProgress(Action<ProgressResponseData[]> onSuccess, Action<string> onFailure)
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            onFailure?.Invoke("Authorization required");
            yield break;
        }

        string url = BASE_API_URL + "progress/my";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ProgressResponseData[] response = JsonHelper.FromJson<ProgressResponseData>(request.downloadHandler.text);
                onSuccess?.Invoke(response);
            }
            else
            {
                onFailure?.Invoke(GetErrorMessage(request));
            }
        }
    }

    // Сохранение прогресса
    public IEnumerator SaveProgress(int objectId, int scores,
                                  Action<ProgressResponseData> onSuccess, Action<string> onFailure)
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            onFailure?.Invoke("Authorization required");
            yield break;
        }

        string url = BASE_API_URL + "progress/my";

        ProgressRequestData requestData = new ProgressRequestData
        {
            object_id = objectId,
            scores = scores
        };

        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ProgressResponseData response = JsonUtility.FromJson<ProgressResponseData>(request.downloadHandler.text);
                onSuccess?.Invoke(response);
            }
            else
            {
                onFailure?.Invoke(GetErrorMessage(request));
            }
        }
    }

    // Удаление аккаунта
    public IEnumerator DeleteAccount(Action<AccountDeleteResponse> onSuccess, Action<string> onFailure)
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            onFailure?.Invoke("Authorization required");
            yield break;
        }

        string url = BASE_API_URL + "account";

        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AccountDeleteResponse response = JsonUtility.FromJson<AccountDeleteResponse>(request.downloadHandler.text);
                if (response.status == "success")
                {
                    _accessToken = null;
                    onSuccess?.Invoke(response);
                }
                else
                {
                    onFailure?.Invoke("Account deletion failed: " + response.message);
                }
            }
            else
            {
                onFailure?.Invoke(GetErrorMessage(request));
            }
        }
    }

    private string GetErrorMessage(UnityWebRequest request)
    {
        if (request.responseCode == 401 && !string.IsNullOrEmpty(request.downloadHandler.text))
        {
            ApiErrorResponse error = JsonUtility.FromJson<ApiErrorResponse>(request.downloadHandler.text);
            return error?.detail ?? "Invalid credentials";
        }

        return request.error ?? $"HTTP error {request.responseCode}";
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.items;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}


