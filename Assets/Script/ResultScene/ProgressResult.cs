using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProgressResult : MonoBehaviour
{
    [Header("UI")]
    public GameObject rowPrefab;
    public GameObject headerPrefab;
    public Transform tableContent;
    public Button loadButton;
    public TMP_Text statusText;

    [Header("Предметы и классификации")]
    private Dictionary<int, string> objectToClassification = new()
    {
        { 1, "Программирование" },
        { 2, "Программирование" },
        { 3, "Базы данных" },
        { 4, "Базы данных" },
        { 5, "Математика" },
        { 6, "Математика" },
        { 7, "Системное администрирование" }
    };

    private Dictionary<int, string> userNames = new();
    private Dictionary<(int userId, string classification), int> groupedScores = new();

    private void Start()
    {
        loadButton.onClick.AddListener(LoadTable);
        if (headerPrefab != null && tableContent.childCount == 0)
            Instantiate(headerPrefab, tableContent);
    }

    void LoadTable()
    {
        for (int i = tableContent.childCount - 1; i > 0; i--)
            Destroy(tableContent.GetChild(i).gameObject);

        groupedScores.Clear();
        userNames.Clear();

        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        yield return StartCoroutine(LoadUserNames());

        foreach (var objectId in objectToClassification.Keys)
        {
            yield return StartCoroutine(LoadProgressByObject(objectId));
        }

        foreach (var entry in groupedScores.OrderBy(e => e.Key.userId))
        {
            string name = userNames.ContainsKey(entry.Key.userId) ? userNames[entry.Key.userId] : $"Пользователь {entry.Key.userId}";
            string classification = entry.Key.classification;
            int score = entry.Value;

            AddRow(name, classification, score.ToString());
        }

        statusText.text = $"Обновлено: {DateTime.Now:T}";
    }

    IEnumerator LoadUserNames()
    {
        string url = "https://gameapi.gd-alt.ru/api/users";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerSession.AccessToken);
        request.SetRequestHeader("accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string wrapped = "{\"items\":" + request.downloadHandler.text + "}";
            var wrapper = JsonUtility.FromJson<UserWrapper>(wrapped);
            foreach (var user in wrapper.items)
                userNames[user.id] = user.name;
        }
        else
        {
            Debug.LogError("Не удалось загрузить пользователей: " + request.error);
        }
    }

    IEnumerator LoadProgressByObject(int objectId)
    {
        string url = $"https://gameapi.gd-alt.ru/api/progress/object/{objectId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerSession.AccessToken);
        request.SetRequestHeader("accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string wrapped = "{\"items\":" + request.downloadHandler.text + "}";
            var wrapper = JsonUtility.FromJson<ProgressWrapper>(wrapped);

            foreach (var record in wrapper.items)
            {
                string classification = objectToClassification[objectId];
                var key = (record.user_id, classification);

                if (!groupedScores.ContainsKey(key))
                    groupedScores[key] = 0;

                groupedScores[key] += record.scores;
            }
        }
        else
        {
            Debug.LogError($"Ошибка загрузки object_id {objectId}: {request.error}");
        }
    }

    void AddRow(string user, string classification, string score)
    {
        GameObject row = Instantiate(rowPrefab, tableContent);
        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();
        if (texts.Length >= 3)
        {
            texts[0].text = user;
            texts[1].text = classification;
            texts[2].text = score;
        }
    }

    [Serializable]
    public class User
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class UserWrapper
    {
        public User[] items;
    }

    [Serializable]
    public class ProgressWrapper
    {
        public ApiManager.ProgressResponseData[] items;
    }
}
