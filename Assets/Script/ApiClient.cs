using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    private const string BASE_URL = "https://gameapi.gd-alt.ru/api/";
    private string _accessToken;

    [Serializable]
    public class LoginResponse { public string access_token; public string token_type; public string user_name; public string role; public int user_id; }
    [Serializable]
    public class RegisterRequest { public int level_type_id; public int role_id; public int specialty_id; public string name; public string login; public string password; }
    [Serializable]
    public class UserCreate { public string name; public string login; public string password; public int role_id; public int specialty_id; public int level_type_id; }
    [Serializable]
    public class UserUpdate { public string name; }
    [Serializable]
    public class Classification { public int id; public string name; }
    [Serializable]
    public class Cabinet { public int id; public string number_cabinet; }
    [Serializable]
    public class LevelType { public int id; public string name; }
    [Serializable]
    public class Specialty { public int id; public string name; }
    [Serializable]
    public class Role { public int id; public string name; }
    [Serializable]
    public class ObjectCreate { public string name; public int cabinet_id; public int classification_id; }
    [Serializable]
    public class ProgressData { public int object_id; public int scores; }

    private void SetAuthHeader(UnityWebRequest request)
    {
        if (!string.IsNullOrEmpty(_accessToken))
            request.SetRequestHeader("Authorization", $"Bearer {_accessToken}");
    }

    public IEnumerator Login(string login, string password, Action<LoginResponse> onSuccess, Action<string> onError)
    {
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "password");
        form.AddField("username", login);
        form.AddField("password", password);
        form.AddField("scope", "");
        form.AddField("client_id", "unity_client");
        form.AddField("client_secret", "secret");

        using UnityWebRequest request = UnityWebRequest.Post(BASE_URL + "auth/login", form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var result = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
            _accessToken = result.access_token;
            onSuccess?.Invoke(result);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    public IEnumerator Register(UserCreate user, Action<string> onSuccess, Action<string> onError)
    {
        string json = JsonUtility.ToJson(user);
        UnityWebRequest request = new UnityWebRequest(BASE_URL + "auth/register", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }
        else
        {
            onError?.Invoke(request.error);
        }
    }

    public IEnumerator GetCurrentUserInfo(Action<string> onSuccess, Action<string> onError)
    {
        UnityWebRequest request = UnityWebRequest.Get(BASE_URL + "auth/me");
        SetAuthHeader(request);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(request.downloadHandler.text);
        else
            onError?.Invoke(request.error);
    }

    public IEnumerator GetAllClassifications(Action<string> onSuccess, Action<string> onError)
    {
        UnityWebRequest request = UnityWebRequest.Get(BASE_URL + "classifications/");
        SetAuthHeader(request);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(request.downloadHandler.text);
        else
            onError?.Invoke(request.error);
    }

    public IEnumerator CreateClassification(string name, Action<string> onSuccess, Action<string> onError)
    {
        string url = BASE_URL + "classifications/?name=" + UnityWebRequest.EscapeURL(name);
        UnityWebRequest request = UnityWebRequest.Post(url, "");
        SetAuthHeader(request);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(request.downloadHandler.text);
        else
            onError?.Invoke(request.error);
    }

    public IEnumerator DeleteUser(int userId, Action<string> onSuccess, Action<string> onError)
    {
        UnityWebRequest request = UnityWebRequest.Delete(BASE_URL + $"users/{userId}");
        SetAuthHeader(request);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(request.downloadHandler.text);
        else
            onError?.Invoke(request.error);
    }

    public IEnumerator PostObject(ObjectCreate objData, Action<string> onSuccess, Action<string> onError)
    {
        string json = JsonUtility.ToJson(objData);
        UnityWebRequest request = new UnityWebRequest(BASE_URL + "objects/", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        SetAuthHeader(request);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(request.downloadHandler.text);
        else
            onError?.Invoke(request.error);
    }

    public IEnumerator GetUserProgress(Action<string> onSuccess, Action<string> onError)
    {
        UnityWebRequest request = UnityWebRequest.Get(BASE_URL + "progress/my/");
        SetAuthHeader(request);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(request.downloadHandler.text);
        else
            onError?.Invoke(request.error);
    }

    public IEnumerator SaveUserProgress(ProgressData data, Action<string> onSuccess, Action<string> onError)
    {
        string json = JsonUtility.ToJson(data);
        UnityWebRequest request = new UnityWebRequest(BASE_URL + "progress/my/", "POST");
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        SetAuthHeader(request);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            onSuccess?.Invoke(request.downloadHandler.text);
        else
            onError?.Invoke(request.error);
    }
}