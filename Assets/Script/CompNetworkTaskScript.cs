using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CompNetworkTaskScript : MonoBehaviour
{
    public TMP_InputField AnswerField;
    public TMP_Text ResultText;
    public Button SaveButton;

    private const string CorrectAnswer = "192.168.1.10"; // ���������� IP
    private const int MaxScore = 50;
    

    private void Start()
    {
        // ������������ ����: ����� � �����
        AnswerField.onValidateInput += ValidateInput;
        SaveButton.onClick.AddListener(CheckAnswer);
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        return (char.IsDigit(addedChar) || addedChar == '.') ? addedChar : '\0';
    }

    private void CheckAnswer()
    {
        string userAnswer = AnswerField.text.Trim();

        int score = userAnswer == CorrectAnswer ? MaxScore : 0;

        ResultText.text = score == MaxScore
            ? $"�������! {score} ������"
            : $"�������! �����: {score}";

        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(7, score, 1,
                onSuccess: data => Debug.Log("���������: " + data.scores),
                onFailure: err => Debug.LogError("������: " + err)
            )
        );
    }
}

