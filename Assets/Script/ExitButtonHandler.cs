using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ExitButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private ApiManager apiManager;

    private void Start()
    {
        if (confirmationPanel == null) Debug.LogError("ConfirmationPanel �� ��������!");
        if (messageText == null) Debug.LogError("MessageText �� ��������!");
        if (yesButton == null || noButton == null) Debug.LogError("������ �� ���������!");

        confirmationPanel?.SetActive(false);
        yesButton?.onClick.AddListener(OnYesClicked);
        noButton?.onClick.AddListener(OnNoClicked);
    }

    public void OnExitButtonPressed()
    {
        messageText.text = "�� ����� ������ �����? ������ ����� ������ ����� �������.";
        confirmationPanel.SetActive(true);
    }

    private void OnYesClicked()
    {
        StartCoroutine(apiManager.DeleteAccount(
            onSuccess: (response) =>
            {
                Debug.Log("������� �����: " + response.name);
                SceneManager.LoadScene("MenuScene");
            },
            onFailure: (error) =>
            {
                Debug.LogError("������ ��� �������� ��������: " + error);
                confirmationPanel.SetActive(false);
            }
        ));
    }

    private void OnNoClicked()
    {
        confirmationPanel.SetActive(false);
    }
}