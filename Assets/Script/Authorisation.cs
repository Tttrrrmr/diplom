using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using System;
using System.Text.RegularExpressions;

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
        EnterButton.onClick.AddListener(OnLoginClicked);
        BackButton.onClick.AddListener(OnBackClicked);
        PasswordField.contentType = TMP_InputField.ContentType.Password;
    }

    private void OnLoginClicked()
    {
        try
        {
            string username = LoginField.text.Trim();
            string password = PasswordField.text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("����� � ������ �� ����� ���� �������");
                return;
            }

            if (!IsValid(username) || !IsValid(password))
            {
                ShowError("��������� ������ ����� �������� � �����, ��� �������� � ������������.");
                return;
            }

            StartCoroutine(LoginCoroutine(username, password));
        }
        catch (Exception ex)
        {
            ShowError("������ �����: " + ex.Message);
        }
    }

    private IEnumerator LoginCoroutine(string username, string password)
    {
        Auth.SetActive(true);
        EnterButton.interactable = false;

        WWWForm form = new WWWForm();
        form.AddField("grant_type", "password");
        form.AddField("username", username);
        form.AddField("password", password);
        form.AddField("scope", "");
        form.AddField("client_id", clientId);
        form.AddField("client_secret", clientSecret);

        using (UnityWebRequest request = UnityWebRequest.Post(loginUrl, form))
        {
            yield return request.SendWebRequest();

            //Auth.SetActive(false);
            EnterButton.interactable = true;

            if (request.result == UnityWebRequest.Result.Success)
            {
                HandleSuccessfulLogin(request.downloadHandler.text);
            }
            else
            {
                HandleLoginError(request);
            }
        }
    }

    private void HandleSuccessfulLogin(string responseJson)
    {
        try
        {
            var response = JsonUtility.FromJson<LoginResponse>(responseJson);
            PlayerPrefs.SetString("access_token", response.access_token);
            PlayerPrefs.SetInt("user_id", response.user_id);
            PlayerPrefs.SetString("user_name", response.user_name);
            PlayerPrefs.Save();

            // ������������� ���� �� ���������� ����, ���� ����
            if (response.role == "admin") PlayerSession.RoleId = 1;
            else if (response.role == "user") PlayerSession.RoleId = 2;
            else PlayerSession.RoleId = 0;

            SceneManager.LoadScene("AvatarScene");
        }
        catch (Exception e)
        {
            ShowError("������ ��������� ������: " + e.Message);
        }
    }

    private void HandleLoginError(UnityWebRequest request)
    {
        string errorMessage = "������ �����������";

        if (request.responseCode == 401)
        {
            errorMessage = "�������� ����� ��� ������";
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

    private void OnBackClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private bool IsValid(string input)
    {
        return Regex.IsMatch(input, @"^[a-zA-Z0-9]{4,20}$");
    }

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

