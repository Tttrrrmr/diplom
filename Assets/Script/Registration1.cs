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
        // ��������� ����� �����
        PasswordField.contentType = TMP_InputField.ContentType.Password;
        PsbField.contentType = TMP_InputField.ContentType.Password;

        // ��������� ����������� ������
        InitializeSpecialtyDropdown();

        // ��������� ������
        RegButton.onClick.AddListener(OnRegisterClicked);
        BackButton.onClick.AddListener(OnBackClicked);
    }

    private void InitializeSpecialtyDropdown()
    {
        // ����� ����� ��������� ������ �������������� �� API ��� ������������ ��������� ������
        SpesialtyDropdown.ClearOptions();
        SpesialtyDropdown.AddOptions(new System.Collections.Generic.List<string>
        {
            "�����������",
            "����������� ��� � �������������� ����������",
            "���������������� ��������������� �������",
            "������������ ������� � ���������",
            "������� � ��������� �����������������"
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
                ShowError("����������, ��������� ��� ����.");
                return;
            }

            if (!IsValid(name) || !IsValid(login))
            {
                ShowError("��� � ����� ����� ��������� ������ �������� � �����.");
                return;
            }

            if (password != repeatPassword)
            {
                ShowError("������ �� ���������.");
                return;
            }

            if (!IsValid(password))
            {
                ShowError("������ ������ ��������� 4�20 ��������: ������ �������� � �����.");
                return;
            }

            StartCoroutine(RegisterCoroutine(name, login, password, specialtyId));
        }
        catch (Exception ex)
        {
            ShowError("������: " + ex.Message);
        }
    }


    private IEnumerator RegisterCoroutine(string name, string login, string password, int specialtyId)
    {
        // ���������� ��������� ��������
        Reg.SetActive(true);
        RegButton.interactable = false;

        // ��������� ������ ��� �����������
        RegistrationData data = new RegistrationData
        {
            level_type_id = 1, // ������ ��������, ����� ������� ���������
            role_id = 2,       // ������ ��������
            specialty_id = specialtyId,
            name = name,
            login = login,
            password = password
        };

        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // ���������� ������
        using (UnityWebRequest request = new UnityWebRequest(registerUrl, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            // �������� ��������� ��������
            Reg.SetActive(false);
            RegButton.interactable = true;

            if (request.result == UnityWebRequest.Result.Success)
            {
                // ������������ �������� �����������
                HandleSuccessfulRegistration(request.downloadHandler.text);
            }
            else
            {
                // ������������ ������
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
                ShowSuccess("����������� ������ �������!", () =>
                {
                    SceneManager.LoadScene("MenuScene");
                });
            }
            else
            {
                ShowError("������ �����������: " + response.message);
            }
        }
        catch (Exception e)
        {
            ShowError($"������ ��������� ������: {e.Message}");
        }
    }


    private void HandleRegistrationError(UnityWebRequest request)
    {
        string errorMessage = "������ �����������";

        if (request.responseCode == 400) // Bad Request
        {
            try
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                errorMessage = errorResponse.detail ?? "������������ ������";
            }
            catch
            {
                errorMessage = "������������ ������";
            }
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            errorMessage = "������ ����������� � �������";
        }

        ShowError(errorMessage);
    }

    private void ShowError(string message)
    {
        var alert = FindObjectOfType<AlertUI>();
        if (alert != null) alert.Show(message);
        else Debug.LogWarning("������ (� AlertUI �� ������): " + message);
    }

    private void ShowSuccess(string message, Action callback = null)
    {
        var alert = FindObjectOfType<AlertUI>();
        if (alert != null) alert.Show(message, callback);
        else
        {
            Debug.Log(message);
            callback?.Invoke(); // �� ����� ���������
        }
    }



    private void OnBackClicked()
    {
        // ������� �� ���������� �����
        SceneManager.LoadScene("MenuScene");
    }

    // ������ ������ ��� API
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
        return Regex.IsMatch(input, @"^[a-zA-Z0-9]{4,20}$"); // �������� + �����, �� 4 �� 20 ��������
    }

}
