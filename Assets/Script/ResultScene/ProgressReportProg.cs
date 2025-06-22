using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class ProgressReportProg : MonoBehaviour
{
    [Header("UI")]
    public GameObject rowPrefab;
    public GameObject headerPrefab;
    public Transform tableContent;
    public Button loadButton; // <- кнопка "Загрузить"
    public TMP_Text statusText;

    [Header("Настройки предмета")]
    public int[] objectIds; // ← список ID заданий
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
        // Удаляем все строки, кроме заголовка (оставляем первый элемент)
        for (int i = tableContent.childCount - 1; i > 0; i--)
        {
            Destroy(tableContent.GetChild(i).gameObject);
        }

        // Загружаем новые строки
        foreach (int id in objectIds)
            StartCoroutine(GetProgressByObject(id));
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
            Debug.LogError($"❌ Ошибка загрузки задания {objectId}: {request.error}");
            statusText.text = $"Ошибка загрузки ID {objectId}";
            yield break;
        }

        ProgressResponseList responseList = JsonUtility.FromJson<ProgressResponseList>("{\"items\":" + request.downloadHandler.text + "}");

        foreach (var record in responseList.items.OrderBy(r => r.user_id).ThenBy(r => r.object_id))
        {
            AddRowToTable(record);
        }

        statusText.text = $"Обновлено: {DateTime.Now:T}";
    }

    void AddRowToTable(ApiManager.ProgressResponseData record)
    {
        GameObject row = Instantiate(rowPrefab, tableContent);
        TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();

        if (texts.Length >= 3)
        {
            string name = (record.user_id == PlayerSession.UserId)
                ? PlayerSession.UserName
                : $"Пользователь {record.user_id}";

            texts[0].text = name;
            texts[1].text = SubjectNameById(record.object_id);
            texts[2].text = record.scores.ToString();
        }
    }

    string SubjectNameById(int id)
    {
        return id switch
        {
            1 => "Основы алгоритмизации и программирования",
            2 => "Разработка программных модулей",
            3 => "UI Элементы",
            4 => "Блок-схемы",
            5 => "Провода",
            6 => "Циклы",
            7 => "Операторы",
            8 => "Сигналы",
            _ => $"Предмет {id}"
        };
    }

    [Serializable]
    public class ProgressResponseList
    {
        public ApiManager.ProgressResponseData[] items;
    }
}