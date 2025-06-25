using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AlgoritmHintScript : MonoBehaviour
{
    [Header("UI Elements")]
    public Button InfoButton;
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button okButton;

    [Header("Scene Settings")]
    public string hintSceneName = "AlgoritmHintScene";

    private void Start()
    {
        popupPanel.SetActive(false);
        InfoButton.onClick.AddListener(OnHintButtonClicked);
        okButton.onClick.AddListener(OnOkClicked);

        // ��������� ������, ���� ��� ��������� �����
        if (HintUsageManager2.UsageCount >= HintUsageManager2.MaxUsage)
        {
            InfoButton.interactable = false;
        }
    }

    private void OnHintButtonClicked()
    {
        if (HintUsageManager2.UsageCount >= HintUsageManager2.MaxUsage)
        {
            InfoButton.interactable = false;
            return;
        }

        if (!HintUsageManager2.HasShownPopup)
        {
            HintUsageManager2.HasShownPopup = true;
            popupPanel.SetActive(true);
            popupText.text = "�� ������ ��������������� ���������� �� ����� 3 ���.";
            return; // ��� ������� ������ "��"
        }

        LoadHintScene();
    }

    private void OnOkClicked()
    {
        popupPanel.SetActive(false);
        LoadHintScene();
    }

    private void LoadHintScene()
    {
        HintUsageManager2.UsageCount++;

        if (HintUsageManager2.UsageCount >= HintUsageManager2.MaxUsage)
        {
            InfoButton.interactable = false;
        }

        SceneManager.LoadScene(hintSceneName);
    }
}
