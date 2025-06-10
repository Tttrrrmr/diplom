using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SchemaChecker : MonoBehaviour
{
    public Button checkButton;
    public TMP_Text resultText;
    public Transform schemaPanel;

    // Правильный порядок по именам объектов
    private List<string> correctOrder = new List<string> {
        "Block_Start",
        "Block_Input",
        "Block_Command",
        "Block_Condition",
        "Block_End"
    };

    void Start()
    {
        checkButton.onClick.AddListener(CheckSchema);
    }

    void CheckSchema()
    {
        List<string> currentOrder = new List<string>();

        Debug.Log("----- Текущий порядок -----");
        foreach (Transform block in schemaPanel)
        {
            currentOrder.Add(block.name);
            Debug.Log(block.name);
        }

        bool correct = true;

        if (currentOrder.Count != correctOrder.Count)
        {
            correct = false;
        }
        else
        {
            for (int i = 0; i < correctOrder.Count; i++)
            {
                if (currentOrder[i] != correctOrder[i])
                {
                    correct = false;
                    break;
                }
            }
        }

        resultText.text = "Результат: " + (correct ? "Правильно!" : "Неправильно") +
                          "\nВаш порядок: " + string.Join(" → ", currentOrder);
    }
}