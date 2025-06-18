using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.SocialPlatforms.Impl;

public class WireGameManager : MonoBehaviour
{
    public Transform panelLeft, panelRight;
    public GameObject wirePrefab;
    public Button checkButton;
    public TMP_Text resultText;
    public TMP_Text timerText;

    private List<WireItem> leftItems = new List<WireItem>();
    private List<WireItem> rightItems = new List<WireItem>();
    private Dictionary<WireItem, WireItem> matches = new Dictionary<WireItem, WireItem>();

    private WireItem selectedLeft = null;
    private Stopwatch timer;

    private string[] colorNames = { "Red", "Green", "Blue", "Yellow" };
    private Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };

    void Start()
    {
        SpawnWires();
        checkButton.onClick.AddListener(CheckMatches);
        timer = new Stopwatch();
        timer.Start();
    }

    void Update()
    {
        timerText.text = $"�����: {timer.Elapsed.TotalSeconds:F2} ���";
    }

    void SpawnWires()
    {
        for (int i = 0; i < colorNames.Length; i++)
        {
            int index = i;

            var left = Instantiate(wirePrefab, panelLeft);
            var leftScript = left.GetComponent<WireItem>();
            leftScript.Setup(colorNames[i], colors[i], item => SelectLeft(item));
            leftItems.Add(leftScript);

            var right = Instantiate(wirePrefab, panelRight);
            var rightScript = right.GetComponent<WireItem>();
            rightScript.Setup(colorNames[i], colors[i], item => SelectRight(item));
            rightItems.Add(rightScript);
        }

        Shuffle(rightItems);
        for (int i = 0; i < rightItems.Count; i++)
            rightItems[i].transform.SetSiblingIndex(i);
    }

    void SelectLeft(WireItem item)
    {
        selectedLeft = item;
    }

    void SelectRight(WireItem item)
    {
        if (selectedLeft != null)
        {
            if (selectedLeft.colorId == item.colorId)
            {
                matches[selectedLeft] = item;
                selectedLeft.GetComponent<Image>().color = Color.gray;
                item.GetComponent<Image>().color = Color.gray;
                selectedLeft = null;
            }
            else
            {
                resultText.text = "������: ������� �� ���������!";
                selectedLeft = null;
            }
        }
    }

    void CheckMatches()
    {
        if (matches.Count == colorNames.Length)
        {
            timer.Stop();
            double time = timer.Elapsed.TotalSeconds;
            resultText.text = $"�����! �����: {time:F2} ���";

            // �������� � API (���� �����)
            StartCoroutine(
                FindObjectOfType<ApiManager>().SaveProgress(8, Mathf.RoundToInt((float) time),
                    onSuccess: data => Debug.Log("���������: " + data.scores),
                    onFailure: err => Debug.LogError("������: " + err)
                )
            );
        }
        else
        {
            resultText.text = "��������� ����������!";
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
