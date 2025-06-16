using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitButtonHandler : MonoBehaviour
{
    public GameObject confirmationPanel; // ������ �������������
    public Button yesButton;
    public Button noButton;
    public Text messageText;
    public ApiManager apiManager;

    private void Start()
    {
        confirmationPanel.SetActive(false);

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
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
                confirmationPanel.SetActive(false); // ������� ���� ��� ������
            }
        ));
    }

    private void OnNoClicked()
    {
        confirmationPanel.SetActive(false);
    }
}
