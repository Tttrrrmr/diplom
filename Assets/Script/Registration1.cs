using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
using System;



public class Registration1 : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] public TMP_InputField NameField;
    [SerializeField] public TMP_InputField LoginField;
    [SerializeField] public TMP_InputField PasswordField;
    [SerializeField] public TMP_InputField PsbField;
    [SerializeField] public TMP_Dropdown SpesialtyDropdown;
    [SerializeField] public Button RegButton;
    [SerializeField] public Button BackButton;
    [SerializeField] public GameObject Reg;

    [Header("API Settings")]
    [SerializeField] private string registerUrl = "https://gameapi.gd-alt.ru/api/auth/register";

    private void Start()
    {
        // Настройка полей ввода
        PasswordField.contentType = TMP_InputField.ContentType.Password;
        PsbField.contentType = TMP_InputField.ContentType.Password;

        // Настройка выпадающего списка
        InitializeSpecialtyDropdown();

        // Настройка кнопок
        RegButton.onClick.AddListener(OnRegisterClicked);
        BackButton.onClick.AddListener(OnBackClicked);
    }

    private void InitializeSpecialtyDropdown()
    {
        // Здесь можно загрузить список специальностей из API или использовать статичный список
        SpesialtyDropdown.ClearOptions();
        SpesialtyDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "Программист",
            "Разработчик веб и мультимедийных приложений",
            "Интеллектуальные интегрированные системы",
            "Компьютерные системы и комплексы",
            "Сетевое и системное администрирование"
        });
    }

    private void OnRegisterClicked()
    {
        try
        {
            string name = NameField.text.Trim();
            string login = LoginField.text.Trim();
            string password = PasswordField.text.Trim();
            string repeatPassword = PsbField.text.Trim();
            int specialtyId = SpesialtyDropdown.value + 1;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(login) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeatPassword))
            {
                ShowError("Пожалуйста, заполните все поля.");
                return;
            }

            if (!IsValid(name) || !IsValid(login))
            {
                ShowError("Имя и логин могут содержать только латиницу и цифры.");
                return;
            }

            if (password != repeatPassword)
            {
                ShowError("Пароли не совпадают.");
                return;
            }

            if (!IsValid(password))
            {
                ShowError("Пароль должен содержать 4–20 символов: только латиница и цифры.");
                return;
            }

            StartCoroutine(RegisterCoroutine(name, login, password, specialtyId));
        }
        catch (Exception ex)
        {
            ShowError("Ошибка: " + ex.Message);
        }
    }


    private IEnumerator RegisterCoroutine(string name, string login, string password, int specialtyId)
    {
        // Показываем индикатор загрузки
        Reg.SetActive(true);
        RegButton.interactable = false;

        // Формируем данные для регистрации
        RegistrationData data = new RegistrationData
        {
            level_type_id = 1, // Пример значения, можно сделать настройку
            role_id = 2,       // Пример значения
            specialty_id = specialtyId,
            name = name,
            login = login,
            password = password
        };

        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // Отправляем запрос
        using (UnityWebRequest request = new UnityWebRequest(registerUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            // Скрываем индикатор загрузки
            Reg.SetActive(false);
            RegButton.interactable = true;

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Обрабатываем успешную регистрацию
                HandleSuccessfulRegistration(request.downloadHandler.text);
            }
            else
            {
                // Обрабатываем ошибку
                HandleRegistrationError(request);
            }
        }
    }

    private void HandleSuccessfulRegistration(string responseJson)
    {
        try
        {
            var response = JsonUtility.FromJson<RegistrationResponse>(responseJson);

            if (response.message == "User created successfully")
            {
                ShowSuccess("Регистрация прошла успешно!", () =>
                {
                    SceneManager.LoadScene("MenuScene");
                });
            }
            else
            {
                ShowError("Ошибка регистрации: " + response.message);
            }
        }
        catch (Exception e)
        {
            ShowError($"Ошибка обработки ответа: {e.Message}");
        }
    }


    private void HandleRegistrationError(UnityWebRequest request)
    {
        string errorMessage = "Ошибка регистрации";

        if (request.responseCode == 400) // Bad Request
        {
            try
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                errorMessage = errorResponse.detail ?? "Некорректные данные";
            }
            catch
            {
                errorMessage = "Некорректные данные";
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
        var alert = FindObjectOfType<AlertUI>();
        if (alert != null) alert.Show(message);
        else Debug.LogWarning("Ошибка (и AlertUI не найден): " + message);
    }

    private void ShowSuccess(string message, Action callback = null)
    {
        var alert = FindObjectOfType<AlertUI>();
        if (alert != null) alert.Show(message, callback);
        else
        {
            Debug.Log(message);
            callback?.Invoke(); // всё равно продолжим
        }
    }



    private void OnBackClicked()
    {
        // Возврат на предыдущую сцену
        SceneManager.LoadScene("MenuScene");
    }

    // Модели данных для API
    [System.Serializable]
    private class RegistrationData
    {
        public int level_type_id;
        public int role_id;
        public int specialty_id;
        public string name;
        public string login;
        public string password;
    }

    [System.Serializable]
    private class RegistrationResponse
    {
        public string message;
        public int user_id;
    }

    [System.Serializable]
    private class ErrorResponse
    {
        public string detail;
    }
    private bool IsValid(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z0-9]{4,20}$"); // латиница + цифры, от 4 до 20 символов
    }

}
