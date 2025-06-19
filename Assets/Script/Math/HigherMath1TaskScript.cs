using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HigherMath1TaskScript : MonoBehaviour
{
    public TMP_InputField AField;
    public TMP_InputField BField;
    public TMP_Text ResultText;
    public Button SaveButton;

    private void Start()
    {
        // Ограничиваем ввод только цифрами и знаком "-"
        AField.onValidateInput += ValidateNumericInput;
        BField.onValidateInput += ValidateNumericInput;

        SaveButton.onClick.AddListener(CheckAnswer);
    }

    private char ValidateNumericInput(string text, int charIndex, char addedChar)
    {
        // Допускаем цифры, минус и пустую строку
        if (char.IsDigit(addedChar) || addedChar == '-') return addedChar;
        return '\0'; // неразрешённый символ
    }

    private void CheckAnswer()
    {
        if (!int.TryParse(AField.text, out int aAnswer) || !int.TryParse(BField.text, out int bAnswer))
        {
            ResultText.text = "Ошибка ввода! Введите только целые числа.";
            return;
        }

        // Ожидаемые правильные значения
        int correctA = -2;
        int correctB = 3;
        int score = 0;

        if (aAnswer == correctA && bAnswer == correctB)
        {
            score = 25;
            ResultText.text = "Успешно! 25 баллов";
        }
        else
        {
            ResultText.text = $"Неверно! Баллы: {score}";
        }

        // Отправка результата в API
        StartCoroutine(
               FindObjectOfType<ApiManager>().SaveProgress(5, score, 1,
                   onSuccess: data => Debug.Log("Сохранено: " + data.scores),
                   onFailure: err => Debug.LogError("Ошибка: " + err)
               )
           );
    }
}
