using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIElementManager2 : MonoBehaviour
{
    public static UIElementManager2 Instance;

    public Button submitButton;
    public TMP_Text resultText;
    public float maxScore = 25f;

    [Header("ID задания для API")]
    public int taskId = 1;

    [Header("Правильные элементы")]
    public List<string> correctElements;

    private HashSet<string> selectedElements = new();

    void Awake() => Instance = this;

    void Start() => submitButton.onClick.AddListener(CheckAnswer);

    public void UpdateSelection(string element, bool isSelected)
    {
        if (isSelected)
            selectedElements.Add(element);
        else
            selectedElements.Remove(element);
    }

    void CheckAnswer()
    {
        int correctSelected = selectedElements.Count(e => correctElements.Contains(e));
        int incorrectSelected = selectedElements.Count(e => !correctElements.Contains(e));
        int missedCorrect = correctElements.Count(e => !selectedElements.Contains(e));

        int totalCorrect = correctElements.Count;

        float correctness = (float)correctSelected / totalCorrect;
        float penalty = incorrectSelected * 0.2f; // 1 ошибка = –20%
        float rawScore = Mathf.Clamp01(correctness - penalty);

        float score = Mathf.Round(rawScore * maxScore * 100f) / 100f;

        resultText.text = $"✅ Правильных: {correctSelected}/{totalCorrect}\n❌ Ошибок: {incorrectSelected}\n🏅 Баллы: {score}";

        LockAllSelectableElements();
        StartCoroutine(
            FindObjectOfType<ApiManager>().SaveProgress(taskId, Mathf.RoundToInt(score), 1,
                onSuccess: data => Debug.Log("Сохранено: " + data.scores),
                onFailure: err => Debug.LogError("Ошибка: " + err)
    )
);
    }

    void LockAllSelectableElements()
    {
        foreach (SelectableElement el in FindObjectsOfType<SelectableElement>())
        {
            el.LockInteraction();
        }
    }
}