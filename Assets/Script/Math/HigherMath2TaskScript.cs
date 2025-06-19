using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HigherMath2TaskScript : MonoBehaviour
{
    public TMP_InputField AnswerField;
    public TMP_Text ResultText;
    public Button SaveButton;

    private const int CorrectAnswer = -5;
    private const int MaxScore = 25;
    

    private void Start()
    {
        // ������������ ���� ������ ������� � �������
        AnswerField.onValidateInput += ValidateNumericInput;

        // ���������� ��������� ������
        SaveButton.onClick.AddListener(CheckAnswer);
    }

    private char ValidateNumericInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) || (charIndex == 0 && addedChar == '-'))
            return addedChar;
        return '\0'; // ������ ��������� ��������
    }

    private void CheckAnswer()
    {
        if (!int.TryParse(AnswerField.text, out int userAnswer))
        {
            ResultText.text = "������ �����! ������� ������ ����� �����.";
            return;
        }

        int score = userAnswer == CorrectAnswer ? MaxScore : 0;

        ResultText.text = userAnswer == CorrectAnswer
            ? $"�������! {score} ������"
            : $"�������! �����: {score}";

        // ���������� ��������� � API
        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(5, score, 2,
                onSuccess: data => Debug.Log("���������: " + data.scores),
                onFailure: err => Debug.LogError("������: " + err)
            )
        );
    }
}
