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
        // Ограничиваем ввод только цифрами и минусом
        AnswerField.onValidateInput += ValidateNumericInput;

        // Подключаем обработку кнопки
        SaveButton.onClick.AddListener(CheckAnswer);
    }

    private char ValidateNumericInput(string text, int charIndex, char addedChar)
    {
        if (char.IsDigit(addedChar) || (charIndex == 0 && addedChar == '-'))
            return addedChar;
        return '\0'; // запрет остальных символов
    }

    private void CheckAnswer()
    {
        if (!int.TryParse(AnswerField.text, out int userAnswer))
        {
            ResultText.text = "Ошибка ввода! Введите только целые числа.";
            return;
        }

        int score = userAnswer == CorrectAnswer ? MaxScore : 0;

        ResultText.text = userAnswer == CorrectAnswer
            ? $"Успешно! {score} баллов"
            : $"Неверно! Баллы: {score}";

        // Отправляем результат в API
        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(5, score, 2,
                onSuccess: data => Debug.Log("Сохранено: " + data.scores),
                onFailure: err => Debug.LogError("Ошибка: " + err)
            )
        );
    }
}
