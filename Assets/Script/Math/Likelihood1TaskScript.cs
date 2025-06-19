using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Likelihood1TaskScript : MonoBehaviour
{
    public TMP_InputField AnswerField;
    public TMP_Text ResultText;
    public Button SaveButton;

    private const string CorrectAnswer = "0,999"; // Точно с запятой
    private const int MaxScore = 25;
  

    private void Start()
    {
        // Ограничение ввода: только цифры и запятая
        AnswerField.onValidateInput += ValidateDecimalInput;

        // Обработка кнопки
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
            ? $"Успешно! {score} баллов"
            : $"Неверно! Баллы: {score}";

        // Отправка в API
        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(6, score, 1,
                onSuccess: data => Debug.Log("Сохранено: " + data.scores),
                onFailure: err => Debug.LogError("Ошибка: " + err)
            )
        );
    }
}
