using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

public class Authorisation : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] public TMP_InputField LoginField;
    [SerializeField] public TMP_InputField PasswordField;
    [SerializeField] public Button EnterButton;
    [SerializeField] public Button BackButton;
    [SerializeField] public GameObject Auth;

    [Header("API Settings")]
    [SerializeField] private string loginUrl = "https://gameapi.gd-alt.ru/api/auth/login";
    [SerializeField] private string clientId = "unity_client";
    [SerializeField] private string clientSecret = "secret";

    private void Start()
    {
        // Настройка кнопок
        EnterButton.onClick.AddListener(OnLoginClicked);
        BackButton.onClick.AddListener(OnBackClicked);

        // Настройка полей ввода
        PasswordField.contentType = TMP_InputField.ContentType.Password;

        // Скрываем индикатор загрузки
        // Auth.SetActive(false);
    }

    private void OnLoginClicked()
    {
        string username = LoginField.text;
        string password = PasswordField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Логин и пароль не могут быть пустыми");
            return;
        }

        StartCoroutine(LoginCoroutine(username, password));
    }

    private IEnumerator LoginCoroutine(string username, string password)
    {
        // Показываем индикатор загрузки
        Auth.SetActive(true);
        EnterButton.interactable = false;

        // Формируем данные для авторизации
        WWWForm form = new WWWForm();
        form.AddField("grant_type", "password");
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("scope", "");
        form.AddField("client_id", clientId);
        form.AddField("client_secret", clientSecret);

        // Отправляем запрос
        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
        {
            yield return request.SendWebRequest();

            // Скрываем индикатор загрузки
            Auth.SetActive(false);
            EnterButton.interactable = true;

            if (request.result == UnityWebRequest.Result.Success) // Если авторизация успешна
            {
                // Обрабатываем успешную авторизацию
                HandleSuccessfulLogin(request.downloadHandler.text); // Выполнить эту функцию с текстом из ответа
            }
            else
            {
                // Обрабатываем ошибку
                HandleLoginError(request);
            }
        }
    }

    private void HandleSuccessfulLogin(string responseJson)
    {
        try
        {
            // Парсим ответ
            var response = JsonUtility.FromJson<LoginResponse>(responseJson);

            // Сохраняем токен (можно использовать PlayerPrefs или другой менеджер)
            PlayerPrefs.SetString("access_token", response.access_token);
            PlayerPrefs.SetInt("user_id", response.user_id);
            PlayerPrefs.SetString("user_name", response.user_name);
            PlayerPrefs.Save(); // 

            // Переходим на главный экран
            SceneManager.LoadScene("AvatarScene");
        }
        catch (System.Exception e)
        {
            ShowError($"Ошибка обработки ответа: {e.Message}");
        }
    }

    private void HandleLoginError(UnityWebRequest request)
    {
        string errorMessage = "Ошибка авторизации";

        if (request.responseCode == 401) // Unauthorized
        {
            try
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                errorMessage = errorResponse.detail ?? "Неверный логин или пароль";
            }
            catch
            {
                errorMessage = "Неверный логин или пароль";
            }
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            errorMessage = "Ошибка подключения к серверу";
        }

        ShowError(errorMessage);
    }

    private void ShowError(string message)
    {
        // Здесь можно реализовать показ ошибки пользователю
        Debug.LogError(message);
        // Например, показать всплывающее окно или текст на экране
    }

    private void OnBackClicked()
    {
        // Возврат на предыдущую сцену
        SceneManager.LoadScene("MenuScene");
    }

    // Модели для ответов API
    [System.Serializable]
    private class LoginResponse
    {
        public string token_type;
        public int user_id;
        public string user_name;
        public string role;
        public string access_token;
    }

    [System.Serializable]
    private class ErrorResponse
    {
        public string detail;
    }
}
