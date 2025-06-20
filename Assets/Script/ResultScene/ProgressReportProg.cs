using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System;

public class ProgressReportProg : MonoBehaviour
{
    [Header("UI")]
    public GameObject rowPrefab;
    public Transform tableContent;
    public Button loadButton; // <- кнопка "Загрузить"
    public TMP_Text statusText;

    [Header("Настройки предмета")]
    public int objectId;
    public string subjectName;

    private void Start()
    {
        if (loadButton != null)
            loadButton.onClick.AddListener(OnLoadClicked);

        if (statusText != null)
            statusText.text = "Нажмите 'Загрузить', чтобы показать результаты.";
    }

    private void OnLoadClicked()
    {
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
            Debug.LogError("❌ Ошибка загрузки данных: " + request.error);
            statusText.text = "Ошибка загрузки данных.";
            yield break;
        }

        ProgressResponseList responseList = JsonUtility.FromJson<ProgressResponseList>("{\"items\":" + request.downloadHandler.text + "}");

        foreach (Transform child in tableContent)
            Destroy(child.gameObject);

        foreach (var record in responseList.items)
        {
            AddRowToTable(record);
        }

        statusText.text = $"Загружено записей: {responseList.items.Length}";
    }

    void AddRowToTable(ApiManager.ProgressResponseData record)
    {
        GameObject row = Instantiate(rowPrefab, tableContent);
        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

        if (texts.Length >= 3)
        {
            // Имя: если текущий пользователь, то по имени, иначе по ID
            string name = (record.user_id == PlayerSession.UserId)
                ? PlayerSession.UserName
                : $"Пользователь {record.user_id}";

            texts[0].text = name;
            texts[1].text = subjectName;
            texts[2].text = record.scores.ToString();
        }
    }

    [Serializable]
    public class ProgressResponseList
    {
        public ApiManager.ProgressResponseData[] items;
    }
}