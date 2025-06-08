using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject answerButton;  // Кнопка "Пока не уверен..."
    public GameObject nextButton;    // Кнопка "Да, поехали!"

    private string[] dialogueLines = {
        "Приветствую, будущий повелитель кода!",
        "Я — Аврора, твой проводник в мир программирования.",
        "Готов ли ты отправиться в увлекательное путешествие?"
    };

    private int currentLine = 0;

    void Start()
    {
        ShowCurrentLine();
    }

    public void OnNextClicked()
    {
        currentLine++;

        if (currentLine < dialogueLines.Length)
        {
            ShowCurrentLine();

            // Прячем кнопку ответа после первого клика
            if (currentLine == 1 && answerButton != null)
                answerButton.SetActive(false);
        }
        else
        {
            // Последняя строка — можно скрыть next
            nextButton.SetActive(false);
        }
    }

    public void OnAnswerClicked()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void ShowCurrentLine()
    {
        dialogueText.text = dialogueLines[currentLine];
    }
}
