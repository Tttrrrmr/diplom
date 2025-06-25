using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MathHintScript : MonoBehaviour
{
    [Header("UI Elements")]
    public Button InfoButton;
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button okButton;

    [Header("Scene Settings")]
    public string hintSceneName = "AdditionHintScene";

    private void Start()
    {
        popupPanel.SetActive(false);
        InfoButton.onClick.AddListener(OnHintButtonClicked);
        okButton.onClick.AddListener(OnOkClicked);

        // ��������� ������, ���� ��� ��������� �����
        if (HintUsageManager5.UsageCount >= HintUsageManager5.MaxUsage)
        {
            InfoButton.interactable = false;
        }
    }

    private void OnHintButtonClicked()
    {
        if (HintUsageManager5.UsageCount >= HintUsageManager5.MaxUsage)
        {
            InfoButton.interactable = false;
            return;
        }

        if (!HintUsageManager5.HasShownPopup)
        {
            HintUsageManager5.HasShownPopup = true;
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
        HintUsageManager5.UsageCount++;

        if (HintUsageManager5.UsageCount >= HintUsageManager5.MaxUsage)
        {
            InfoButton.interactable = false;
        }

        SceneManager.LoadScene(hintSceneName);
    }
}
