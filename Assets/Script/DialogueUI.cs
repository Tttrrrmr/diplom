using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject answerButton;  // ������ "���� �� ������..."
    public GameObject nextButton;    // ������ "��, �������!"

    private string[] dialogueLines = {
        "�����������, ������� ���������� ����!",
        "� � ������, ���� ��������� � ��� ����������������.",
        "����� �� �� ����������� � ������������� �����������?"
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

            // ������ ������ ������ ����� ������� �����
            if (currentLine == 1 && answerButton != null)
                answerButton.SetActive(false);
        }
        else
        {
            // ��������� ������ � ����� ������ next
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
