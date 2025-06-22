using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class WireSortingGameManual : MonoBehaviour
{
    public TMP_Text taskDescription;
    public Button checkButton;
    public TMP_Text resultText;
    public Transform leftPanel;
    public Transform rightPanel; // ← добавь это поле в класс

    private List<string> correctOrder = new List<string> {
        "Wire_1", "Wire_2", "Wire_3", "Wire_4",
        "Wire_5", "Wire_6", "Wire_7", "Wire_8"
    };

    private float timer;
    private bool isRunning = false;
    float CalculateScore(bool correct, float time, float maxScore)
    {
        if (!correct) return 0;

        if (time <= 10f) return maxScore;
        if (time >= 60f) return 0;

        // Линейное уменьшение от 10 до 60 сек
        float t = (60f - time) / 50f;
        return Mathf.Round(t * maxScore * 100f) / 100f;
    }

    void Start()
    {
        checkButton.onClick.AddListener(CheckOrder);
        ShuffleWires();
        timer = 0f;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
        }
    }

    void ShuffleWires()
    {
        var wires = new List<Transform>();
        foreach (Transform child in leftPanel)
            wires.Add(child);

        var shuffled = wires.OrderBy(x => Random.value).ToList();
        for (int i = 0; i < shuffled.Count; i++)
            shuffled[i].SetSiblingIndex(i);
    }

    void CheckOrder()
    {
        List<string> currentOrder = new List<string>();
        foreach (Transform wire in rightPanel)
        {
            string name = wire.name.Split('(')[0].Trim();
            currentOrder.Add(name);
        }

        bool correct = currentOrder.SequenceEqual(correctOrder);

        isRunning = false;
        float time = Mathf.Round(timer * 100f) / 100f;
        float score = CalculateScore(correct, time, 50f); // максимум 50 баллов

        resultText.text = (correct ? "Правильно!" : "Неправильно!") + $" Время: {time} сек. Баллы: {score}";

        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(13, Mathf.RoundToInt(score), 2,
                onSuccess: data => Debug.Log("Сохранено: " + data.scores),
                onFailure: err => Debug.LogError("Ошибка: " + err)
            )
        );
    }
}