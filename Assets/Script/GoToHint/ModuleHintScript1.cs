using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ModuleHintScript1 : MonoBehaviour
{
    [Header("UI Elements")]
    public Button InfoButton;
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button okButton;

    [Header("Scene Settings")]
    public string hintSceneName = "ModuleHintScene1";

    private void Start()
    {
        popupPanel.SetActive(false);
        InfoButton.onClick.AddListener(OnHintButtonClicked);
        okButton.onClick.AddListener(OnOkClicked);

        // ��������� ������, ���� ��� ��������� �����
        if (HintUsageManager4.UsageCount >= HintUsageManager4.MaxUsage)
        {
            InfoButton.interactable = false;
        }
    }

    private void OnHintButtonClicked()
    {
        if (HintUsageManager4.UsageCount >= HintUsageManager4.MaxUsage)
        {
            InfoButton.interactable = false;
            return;
        }

        if (!HintUsageManager4.HasShownPopup)
        {
            HintUsageManager4.HasShownPopup = true;
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
        HintUsageManager4.UsageCount++;

        if (HintUsageManager4.UsageCount >= HintUsageManager4.MaxUsage)
        {
            InfoButton.interactable = false;
        }

        SceneManager.LoadScene(hintSceneName);
    }
}
