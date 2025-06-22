using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SchemaChecker2 : MonoBehaviour
{
    public Button checkButton;
    public TMP_Text resultText;
    public Transform schemaPanel;

    private List<string> correctOrder = new List<string> {
        "Block_Start",
        "Block_Input",
        "Block_Command",
        "Block_Output",
        "Block_End"
    };

    void Start()
    {
        checkButton.onClick.AddListener(CheckSchema);
    }

    void CheckSchema()
    {
        List<string> currentOrder = new List<string>();
        foreach (Transform block in schemaPanel)
        {
            string name = block.name.Split('(')[0].Trim(); // убираем (Clone)
            currentOrder.Add(name);
        }

        int correctCount = 0;
        for (int i = 0; i < Mathf.Min(currentOrder.Count, correctOrder.Count); i++)
        {
            if (currentOrder[i] == correctOrder[i])
                correctCount++;
        }

        int total = correctOrder.Count;
        int score = Mathf.RoundToInt((correctCount / (float)total) * 25f);

        resultText.text = $"Правильно расположено блоков: {correctCount} из {total}\nБаллы: {score}";

        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(2, score, 1,
                onSuccess: data => Debug.Log("Сохранено: " + data.scores),
                onFailure: err => Debug.LogError("Ошибка: " + err)
            )
        );
    }
}