
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class MatchManagerAccess : MonoBehaviour
{
    public RectTransform panelLeft, panelRight;
    public GameObject itemPrefab;
    public Button checkButton;
    public TextMeshProUGUI resultText;

    private Dictionary<MatchItem, MatchItem> pairs = new();
    private MatchItem selectedLeft;
    private MatchItem selectedRight;
    private Dictionary<string, string> correctMap;

    void Start()
    {
        Time.timeScale = 1;
        var commands = new Dictionary<string, string>
        {
            {"Поле", "Столбец таблицы, хранящий определенный тип данных"},
            {"Запись", "Строка в таблице, представляющая один объект"},
            {"Первичный ключ", "Уникальный идентификатор записи в таблице"},
            {"Внешний ключ", "Поле для связи с первичным ключом другой таблицы"},
            {"Нормализация", "Процесс устранения избыточности данных путем разделения таблиц"},
            {"Справочник", "Таблица, содержащая уникальные значения"},
            {"Индекс", "Структура для ускорения поиска данных"}
        };

        Populate(commands);
        checkButton.onClick.AddListener(CheckResults);
    }

    void Populate(Dictionary<string,string> commands)
    {
        var rnd = commands.OrderBy(_ => Random.value).ToList();
        correctMap = commands;

        foreach (var kv in commands)
        {
            var go = Instantiate(itemPrefab, panelLeft);
            var mi = go.GetComponent<MatchItem>();
            mi.Init(kv.Key, kv.Key);
            mi.Button.onClick.AddListener(() => OnLeftClick(mi));
        }

        foreach (var kv in rnd)
        {
            var go = Instantiate(itemPrefab, panelRight);
            var mi = go.GetComponent<MatchItem>();
            mi.Init(kv.Key, kv.Value);
            mi.Button.onClick.AddListener(() => OnRightClick(mi));
        }
    }

    void OnLeftClick(MatchItem mi)
    {
        if (selectedLeft != null) selectedLeft.Toggle();
        selectedLeft = mi;
        selectedLeft.Toggle();
        TryPair();
    }

    void OnRightClick(MatchItem mi)
    {
        if (selectedRight != null) selectedRight.Toggle();
        selectedRight = mi;
        selectedRight.Toggle();
        TryPair();
    }

    void TryPair()
    {
        if (selectedLeft != null && selectedRight != null)
        {
            pairs[selectedLeft] = selectedRight;
            selectedLeft = null; selectedRight = null;
        }
    }

    void CheckResults()
    {
        int correct = pairs.Count(p => correctMap.ContainsKey(p.Key.Key) && correctMap[p.Key.Key] == p.Value.Text.text);
        float score = Mathf.Round(correct * (50f / correctMap.Count));
        float time = Time.timeSinceLevelLoad;

        resultText.text = $"Правильно: {correct}/{correctMap.Count}\nБаллы: {score}\nВремя: {time:F2} сек";

        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(3, Mathf.RoundToInt(score), 1,
                onSuccess: data => Debug.Log("Сохранено: " + data.scores),
                onFailure: err => Debug.LogError("Ошибка: " + err)
            )
        );
    }

}
