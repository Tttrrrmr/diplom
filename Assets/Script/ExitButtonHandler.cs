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
        if (confirmationPanel == null) Debug.LogError("ConfirmationPanel не назначен!");
        if (messageText == null) Debug.LogError("MessageText не назначен!");
        if (yesButton == null || noButton == null) Debug.LogError("Кнопки не назначены!");

        confirmationPanel?.SetActive(false);
        yesButton?.onClick.AddListener(OnYesClicked);
        noButton?.onClick.AddListener(OnNoClicked);
    }

    public void OnExitButtonPressed()
    {
        messageText.text = "Вы точно хотите выйти? Данные после выхода будут удалены.";
        confirmationPanel.SetActive(true);
    }

    private void OnYesClicked()
    {
        StartCoroutine(apiManager.DeleteAccount(
            onSuccess: (response) =>
            {
                Debug.Log("Аккаунт удалён: " + response.name);
                SceneManager.LoadScene("MenuScene");
            },
            onFailure: (error) =>
            {
                Debug.LogError("Ошибка при удалении аккаунта: " + error);
                confirmationPanel.SetActive(false);
            }
        ));
    }

    private void OnNoClicked()
    {
        confirmationPanel.SetActive(false);
    }
}