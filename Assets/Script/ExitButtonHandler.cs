using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitButtonHandler : MonoBehaviour
{
    public GameObject confirmationPanel; // Панель подтверждения
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
                confirmationPanel.SetActive(false); // Закрыть даже при ошибке
            }
        ));
    }

    private void OnNoClicked()
    {
        confirmationPanel.SetActive(false);
    }
}
