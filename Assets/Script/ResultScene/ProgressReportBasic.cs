using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ProgressReportBasic : MonoBehaviour
{
    [Header("UI")]
    public GameObject rowPrefab;
    public GameObject headerPrefab;
    public Transform tableContent;
    public Button loadButton;
    public TMP_Text statusText;

    [Header("Настройки предмета")]
    public int objectId = 3; // ← ID ПРЕДМЕТА
    public string subjectName = "Основы проектирования базы данных";

    private Dictionary<int, int> userScores = new();
    private Dictionary<int, string> userNames = new();

    private void Start()
    {
        if (loadButton != null)
            loadButton.onClick.AddListener(OnLoadClicked);

        if (statusText != null)
            statusText.text = "Нажмите 'Отчет', чтобы показать результаты.";

        if (tableContent.childCount == 0 && headerPrefab != null)
            Instantiate(headerPrefab, tableContent);
    }

    private void OnLoadClicked()
    {
        for (int i = tableContent.childCount - 1; i > 0; i--)
            Destroy(tableContent.GetChild(i).gameObject);

        userScores.Clear();
        userNames.Clear();

        StartCoroutine(LoadData());
    }

    private IEnumerator LoadData()
    {
        yield return StartCoroutine(LoadUserNames());
        yield return StartCoroutine(LoadProgressByObject(objectId));

        foreach (var entry in userScores)
        {
            string userName = userNames.ContainsKey(entry.Key) ? userNames[entry.Key] : $"Пользователь {entry.Key}";
            AddRow(userName, subjectName, entry.Value.ToString());
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
            UserWrapper wrapper = JsonUtility.FromJson<UserWrapper>(wrapped);

            foreach (var user in wrapper.items)
                userNames[user.id] = user.name;
        }
        else
        {
            Debug.LogError("❌ Не удалось загрузить пользователей: " + request.error);
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
            ProgressWrapper wrapper = JsonUtility.FromJson<ProgressWrapper>(wrapped);

            foreach (var record in wrapper.items)
            {
                // Суммируем баллы по одному object_id (то есть по предмету), вне зависимости от task_number
                if (!userScores.ContainsKey(record.user_id))
                    userScores[record.user_id] = 0;

                userScores[record.user_id] += record.scores;
            }
        }
        else
        {
            Debug.LogError($"❌ Ошибка загрузки object_id {objectId}: {request.error}");
        }
    }

    void AddRow(string userName, string subject, string score)
    {
        GameObject row = Instantiate(rowPrefab, tableContent);
        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

        if (texts.Length >= 3)
        {
            texts[0].text = userName;
            texts[1].text = subject;
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
