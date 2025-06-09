using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using System.Collections.Generic;

public class ApiManager : MonoBehaviour
{
    private const string BASE_API_URL = "https://gameapi.gd-alt.ru/api/";

    private string _accessToken;
    private int _userId;
    private string _userName;
    private string _userRole;

    #region Data Models
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
    public class ProgressData
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

    [Serializable]
    public class Cabinet
    {
        public int id;
        public string number_cabinet;
    }

    [Serializable]
    public class Classification
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class LevelType
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class Role
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class Specialty
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class Object
    {
        public int id;
        public int cabinet_id;
        public int classification_id;
        public string name;
    }
    #endregion

    //регистрация новорого пользователя
    #region Auth Methods
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

        yield return SendRequest(url, "POST", requestData, (responseJson) => {
            RegisterResponseData response = JsonUtility.FromJson<RegisterResponseData>(responseJson);
            onSuccess?.Invoke(response.user_id);
        }, onFailure);
    }

    //авторизация пользователя
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

    public IEnumerator GetCurrentUserInfo(Action<LoginResponseData> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "auth/me";
        yield return SendAuthenticatedRequest(url, "GET", null, (responseJson) => {
            LoginResponseData response = JsonUtility.FromJson<LoginResponseData>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }
    #endregion

    //получение прогресса пользователя
    #region Progress Methods
    public IEnumerator GetUserProgress(Action<ProgressResponseData[]> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "progress/my";
        yield return SendAuthenticatedRequest(url, "GET", null, (responseJson) => {
            ProgressResponseData[] response = JsonHelper.FromJson<ProgressResponseData>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }

    //сохранение прогресса
    public IEnumerator SaveProgress(int objectId, int scores,
                                  Action<ProgressResponseData> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "progress/my";
        ProgressData requestData = new ProgressData
        {
            object_id = objectId,
            scores = scores
        };

        yield return SendAuthenticatedRequest(url, "POST", requestData, (responseJson) => {
            ProgressResponseData response = JsonUtility.FromJson<ProgressResponseData>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }
    #endregion

    #region Data Methods
    public IEnumerator GetCabinets(Action<Cabinet[]> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "cabinets/";
        yield return SendAuthenticatedRequest(url, "GET", null, (responseJson) => {
            Cabinet[] response = JsonHelper.FromJson<Cabinet>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }

    public IEnumerator GetClassifications(Action<Classification[]> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "classifications/";
        yield return SendAuthenticatedRequest(url, "GET", null, (responseJson) => {
            Classification[] response = JsonHelper.FromJson<Classification>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }

    public IEnumerator GetLevelTypes(Action<LevelType[]> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "level-types/";
        yield return SendAuthenticatedRequest(url, "GET", null, (responseJson) => {
            LevelType[] response = JsonHelper.FromJson<LevelType>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }

    public IEnumerator GetRoles(Action<Role[]> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "roles/";
        yield return SendAuthenticatedRequest(url, "GET", null, (responseJson) => {
            Role[] response = JsonHelper.FromJson<Role>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }

    public IEnumerator GetSpecialties(Action<Specialty[]> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "specialties/";
        yield return SendAuthenticatedRequest(url, "GET", null, (responseJson) => {
            Specialty[] response = JsonHelper.FromJson<Specialty>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }

    public IEnumerator GetObjects(Action<Object[]> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "objects/";
        yield return SendAuthenticatedRequest(url, "GET", null, (responseJson) => {
            Object[] response = JsonHelper.FromJson<Object>(responseJson);
            onSuccess?.Invoke(response);
        }, onFailure);
    }
    #endregion

    //удаление аккаунта
    #region Account Management
    public IEnumerator DeleteAccount(Action<AccountDeleteResponse> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "account";
        yield return SendAuthenticatedRequest(url, "DELETE", null, (responseJson) => {
            AccountDeleteResponse response = JsonUtility.FromJson<AccountDeleteResponse>(responseJson);
            _accessToken = null;
            onSuccess?.Invoke(response);
        }, onFailure);
    }
    #endregion

    #region Helper Methods
    private IEnumerator SendRequest(string url, string method, object requestData,
                                  Action<string> onSuccess, Action<string> onFailure)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            if (requestData != null)
            {
                string jsonData = JsonUtility.ToJson(requestData);
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.SetRequestHeader("Content-Type", "application/json");
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onFailure?.Invoke(GetErrorMessage(request));
            }
        }
    }

    private IEnumerator SendAuthenticatedRequest(string url, string method, object requestData,
                                               Action<string> onSuccess, Action<string> onFailure)
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            onFailure?.Invoke("Authorization required");
            yield break;
        }

        yield return SendRequest(url, method, requestData, onSuccess, onFailure);
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
    #endregion
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
