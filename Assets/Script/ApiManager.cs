using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;

public class ApiManager : MonoBehaviour
{
    // Базовый URL API из документации
    private const string BASE_API_URL = "https://gameapi.gd-alt.ru/api/";

    // Переменные для хранения данных пользователя
    private string _accessToken;
    private int _userId;
    private string _userName;
    private string _userRole;

    // Модели данных для запросов и ответов

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
    public class LoginRequestData
    {
        public string grant_type = "password";
        public string username;
        public string password;
        public string scope = "";
        public string client_id = "unity_client";
        public string client_secret = "secret";
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
    public class ApiErrorResponse
    {
        public string detail;
    }

    // Метод регистрации
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

        string jsonRequestBody = JsonUtility.ToJson(requestData);

        UnityWebRequest uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);
        uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (HandleApiResponse(uwr))
        {
            RegisterResponseData response = JsonUtility.FromJson<RegisterResponseData>(uwr.downloadHandler.text);
            if (response != null && response.message == "User created successfully")
            {
                onSuccess?.Invoke(response.user_id);
            }
            else
            {
                onFailure?.Invoke("Ошибка регистрации: неверный формат ответа");
            }
        }
        else
        {
            onFailure?.Invoke(ParseError(uwr));
        }
    }

    // Метод авторизации
    public IEnumerator Login(string username, string password,
                           Action<LoginResponseData> onSuccess, Action<string> onFailure)
    {
        string url = BASE_API_URL + "auth/login";

        // Для URL-encoded данных используем WWWForm
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "password");
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("scope", "");
        form.AddField("client_id", "unity_client");
        form.AddField("client_secret", "secret");

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        uwr.downloadHandler = new DownloadHandlerBuffer();

        yield return uwr.SendWebRequest();

        if (HandleApiResponse(uwr))
        {
            LoginResponseData response = JsonUtility.FromJson<LoginResponseData>(uwr.downloadHandler.text);
            if (response != null && !string.IsNullOrEmpty(response.access_token))
            {
                _accessToken = response.access_token;
                _userId = response.user_id;
                _userName = response.user_name;
                _userRole = response.role;

                onSuccess?.Invoke(response);
            }
            else
            {
                onFailure?.Invoke("Ошибка авторизации: неверный формат ответа");
            }
        }
        else
        {
            onFailure?.Invoke(ParseError(uwr));
        }
    }

    // Получение прогресса пользователя
    public IEnumerator GetUserProgress(Action<ProgressResponseData[]> onSuccess, Action<string> onFailure)
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            onFailure?.Invoke("Требуется авторизация");
            yield break;
        }

        string url = BASE_API_URL + "progress/my";

        UnityWebRequest uwr = UnityWebRequest.Get(url);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Authorization", "Bearer " + _accessToken);

        yield return uwr.SendWebRequest();

        if (HandleApiResponse(uwr))
        {
            ProgressResponseData[] response = JsonHelper.FromJson<ProgressResponseData>(uwr.downloadHandler.text);
            onSuccess?.Invoke(response);
        }
        else
        {
            onFailure?.Invoke(ParseError(uwr));
        }
    }

    // Сохранение прогресса
    public IEnumerator SaveProgress(int objectId, int scores,
                                  Action<ProgressResponseData> onSuccess, Action<string> onFailure)
    {
        if (string.IsNullOrEmpty(_accessToken))
        {
            onFailure?.Invoke("Требуется авторизация");
            yield break;
        }

        string url = BASE_API_URL + "progress/my";

        ProgressData requestData = new ProgressData
        {
            object_id = objectId,
            scores = scores
        };

        string jsonRequestBody = JsonUtility.ToJson(requestData);

        UnityWebRequest uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonRequestBody);
        uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
        uwr.downloadHandler = new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", "Bearer " + _accessToken);

        yield return uwr.SendWebRequest();

        if (HandleApiResponse(uwr))
        {
            ProgressResponseData response = JsonUtility.FromJson<ProgressResponseData>(uwr.downloadHandler.text);
            onSuccess?.Invoke(response);
        }
        else
        {
            onFailure?.Invoke(ParseError(uwr));
        }
    }

    // Обработка ответов API
    private bool HandleApiResponse(UnityWebRequest uwr)
    {
        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError($"Ошибка сети: {uwr.error}");
            return false;
        }
        else if (uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Ошибка HTTP: {uwr.responseCode}");
            return false;
        }
        return true;
    }

    // Парсинг ошибок
    private string ParseError(UnityWebRequest uwr)
    {
        if (!string.IsNullOrEmpty(uwr.downloadHandler.text))
        {
            try
            {
                ApiErrorResponse error = JsonUtility.FromJson<ApiErrorResponse>(uwr.downloadHandler.text);
                if (error != null && !string.IsNullOrEmpty(error.detail))
                {
                    return error.detail;
                }
            }
            catch
            {
                // Если не удалось распарсить JSON с ошибкой
            }
        }
        return uwr.error ?? "Неизвестная ошибка";
    }
}

// Вспомогательный класс для десериализации массивов JSON
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

