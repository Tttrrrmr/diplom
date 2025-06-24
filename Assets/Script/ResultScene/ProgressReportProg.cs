using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;

public class ProgressReportProg : MonoBehaviour
{
    [Header("UI")]
    public GameObject rowPrefab;
    public GameObject headerPrefab;
    public Transform tableContent;
    public Button loadButton;
    public TMP_Text statusText;

    [Header("Настройки предмета")]
    public int objectId; // ← Один предмет
    public string subjectName;

    private void Start()
    {
        if (loadButton != null)
            loadButton.onClick.AddListener(OnLoadClicked);

        if (statusText != null)
            statusText.text = "Нажмите 'Загрузить', чтобы показать результаты.";

        if (tableContent.childCount == 0)
        {
            Instantiate(headerPrefab, tableContent);
        }
    }

    private void OnLoadClicked()
    {
        // Удаляем все строки кроме заголовка
        for (int i = tableContent.childCount - 1; i > 0; i--)
        {
            Destroy(tableContent.GetChild(i).gameObject);
        }

        StartCoroutine(GetProgressByObject(objectId));
    }

    IEnumerator GetProgressByObject(int objectId)
    {
        string token = PlayerSession.AccessToken;

        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("❌ Токен отсутствует!");
            statusText.text = "Вы не авторизованы.";
            yield break;
        }

        string url = $"https://gameapi.gd-alt.ru/api/progress/object/{objectId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + token);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"❌ Ошибка загрузки: {request.error}");
            statusText.text = $"Ошибка загрузки ID {objectId}";
            yield break;
        }

        var responseList = JsonUtility.FromJson<ProgressResponseList>("{\"items\":" + request.downloadHandler.text + "}");

        // Группировка: user_id → сумма scores по task_number
        var grouped = responseList.items
            .Where(r => r.object_id == objectId)
            .GroupBy(r => r.user_id)
            .Select(g => new
            {
                userId = g.Key,
                totalScore = g.Sum(r => r.scores)
            })
            .OrderBy(x => x.userId);

        foreach (var entry in grouped)
        {
            AddRow(entry.userId, subjectName, entry.totalScore);
        }

        statusText.text = $"Обновлено: {DateTime.Now:T}";
    }

    void AddRow(int userId, string subject, int score)
    {
        GameObject row = Instantiate(rowPrefab, tableContent);
        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

        if (texts.Length >= 3)
        {
            string name = (userId == PlayerSession.UserId)
                ? PlayerSession.UserName
                : $"Пользователь {userId}";

            texts[0].text = name;
            texts[1].text = subject;
            texts[2].text = score.ToString();
        }
    }

    [Serializable]
    public class ProgressResponseList
    {
        public ApiManager.ProgressResponseData[] items;
    }
}