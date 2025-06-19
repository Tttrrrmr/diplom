using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Likelihood2TaskScript : MonoBehaviour
{
    public TMP_InputField AnswerField;
    public TMP_Text ResultText;
    public Button SaveButton;

    private const string CorrectAnswer = "0,657"; // Точно с запятой
    private const int MaxScore = 25;
   

    private void Start()
    {
        // Ограничиваем ввод: только цифры и запятая
        AnswerField.onValidateInput += ValidateDecimalInput;

        SaveButton.onClick.AddListener(CheckAnswer);
    }

    private char ValidateDecimalInput(string text, int charIndex, char addedChar)
    {
        return (char.IsDigit(addedChar) || addedChar == ',') ? addedChar : '\0';
    }

    private void CheckAnswer()
    {
        string userAnswer = AnswerField.text.Trim().Replace(".", ",");
        int score = userAnswer == CorrectAnswer ? MaxScore : 0;

        ResultText.text = score == MaxScore
            ? $"Успешно! {score} баллов"
            : $"Неверно! Баллы: {score}";

        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(6, score, 2,
                onSuccess: data => Debug.Log("Сохранено: " + data.scores),
                onFailure: err => Debug.LogError("Ошибка: " + err)
            )
        );
    }
}

