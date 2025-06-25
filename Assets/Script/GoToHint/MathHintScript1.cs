using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MathHintScript1 : MonoBehaviour
{
    [Header("UI Elements")]
    public Button InfoButton;
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button okButton;

    [Header("Scene Settings")]
    public string hintSceneName = "MulHintScene";

    private void Start()
    {
        popupPanel.SetActive(false);
        InfoButton.onClick.AddListener(OnHintButtonClicked);
        okButton.onClick.AddListener(OnOkClicked);

        // ��������� ������, ���� ��� ��������� �����
        if (HintUsageManager6.UsageCount >= HintUsageManager6.MaxUsage)
        {
            InfoButton.interactable = false;
        }
    }

    private void OnHintButtonClicked()
    {
        if (HintUsageManager6.UsageCount >= HintUsageManager6.MaxUsage)
        {
            InfoButton.interactable = false;
            return;
        }

        if (!HintUsageManager6.HasShownPopup)
        {
            HintUsageManager6.HasShownPopup = true;
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
        HintUsageManager6.UsageCount++;

        if (HintUsageManager6.UsageCount >= HintUsageManager6.MaxUsage)
        {
            InfoButton.interactable = false;
        }

        SceneManager.LoadScene(hintSceneName);
    }
}
