using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ProgressReportProg : MonoBehaviour
{
    [Header("UI References")]
    public GameObject rowPrefab;
    public Transform tableContentParent;

    [Header("Config")]
    public int objectId = 5;
    public string subjectName = "Основы алгоритмизации и программирования";

    private const string BASE_API_URL = "https://gameapi.gd-alt.ru/api/";
    private Dictionary<int, string> _userNames = new();

    [Serializable]
    public class User
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class ProgressRecord
    {
        public int id;
        public int user_id;
        public int object_id;
        public int task_number;
        public int scores;
    }

    private void Start()
    {
        StartCoroutine(LoadProgressTable(objectId));
    }

    IEnumerator LoadProgressTable(int objectId)
    {
        yield return StartCoroutine(LoadAllUsers(() =>
        {
            StartCoroutine(GetProgressByObject(objectId));
        }));
    }

    IEnumerator LoadAllUsers(Action onSuccess)
    {
        string url = BASE_API_URL + "users";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerSession.AccessToken);
        request.SetRequestHeader("accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string wrapped = "{\"items\":" + request.downloadHandler.text + "}";
            User[] users = JsonHelper.FromJson<User>(wrapped);
            _userNames.Clear();

            foreach (var user in users)
                _userNames[user.id] = user.name;

            onSuccess?.Invoke();
        }
        else
        {
            Debug.LogError("Не удалось загрузить пользователей: " + request.error);
        }
    }

    IEnumerator GetProgressByObject(int objectId)
    {
        string url = BASE_API_URL + $"progress/object/{objectId}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerSession.AccessToken);
        request.SetRequestHeader("accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string wrapped = "{\"items\":" + request.downloadHandler.text + "}";
            ProgressRecord[] records = JsonHelper.FromJson<ProgressRecord>(wrapped);

            // ⬇ Группировка по пользователям, суммируются все задания
            Dictionary<int, int> userTotalScores = new();

            foreach (var record in records)
            {
                // Проверяем только нужный объект (на всякий случай)
                if (record.object_id != objectId) continue;

                if (!userTotalScores.ContainsKey(record.user_id))
                    userTotalScores[record.user_id] = 0;

                userTotalScores[record.user_id] += record.scores;
            }

            // Очищаем таблицу
            foreach (Transform child in tableContentParent)
                Destroy(child.gameObject);

            // Добавляем строки
            foreach (var kvp in userTotalScores)
            {
                string userName = _userNames.ContainsKey(kvp.Key) ? _userNames[kvp.Key] : $"User {kvp.Key}";
                string score = kvp.Value.ToString();

                AddRowToTable(userName, subjectName, score);
            }
        }
        else
        {
            Debug.LogError("Ошибка загрузки прогресса: " + request.downloadHandler.text);
        }
    }


    void AddRowToTable(string user, string subject, string score)
    {
        GameObject row = Instantiate(rowPrefab, tableContentParent);
        TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();

        if (texts.Length >= 3)
        {
            texts[0].text = user;
            texts[1].text = subject;
            texts[2].text = score;
        }
        else
        {
            Debug.LogWarning("RowPrefab должен содержать три TextMeshProUGUI компонента.");
        }
    }
}

