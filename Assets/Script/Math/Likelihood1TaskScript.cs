using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Likelihood1TaskScript : MonoBehaviour
{
    public TMP_InputField AnswerField;
    public TMP_Text ResultText;
    public Button SaveButton;

    private const string CorrectAnswer = "0,999"; // ����� � �������
    private const int MaxScore = 25;
  

    private void Start()
    {
        // ����������� �����: ������ ����� � �������
        AnswerField.onValidateInput += ValidateDecimalInput;

        // ��������� ������
        SaveButton.onClick.AddListener(CheckAnswer);
    }

    private char ValidateDecimalInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) || addedChar == ',') return addedChar;
        return '\0';
    }

    private void CheckAnswer()
    {
        string userAnswer = AnswerField.text.Replace(" ", "").Replace(".", ",");

        int score = userAnswer == CorrectAnswer ? MaxScore : 0;

        ResultText.text = score > 0
            ? $"�������! {score} ������"
            : $"�������! �����: {score}";

        // �������� � API
        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(6, score, 1,
                onSuccess: data => Debug.Log("���������: " + data.scores),
                onFailure: err => Debug.LogError("������: " + err)
            )
        );
    }
}
