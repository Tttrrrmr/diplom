using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LikelihoodHintScript : MonoBehaviour
{
    [Header("UI Elements")]
    public Button InfoButton;
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button okButton;

    [Header("Scene Settings")]
    public string hintSceneName = "LikelihoodHintScene";

    private void Start()
    {
        popupPanel.SetActive(false);
        InfoButton.onClick.AddListener(OnHintButtonClicked);
        okButton.onClick.AddListener(OnOkClicked);

        // ��������� ������, ���� ��� ��������� �����
        if (HintUsageManager7.UsageCount >= HintUsageManager7.MaxUsage)
        {
            InfoButton.interactable = false;
        }
    }

    private void OnHintButtonClicked()
    {
        if (HintUsageManager7.UsageCount >= HintUsageManager7.MaxUsage)
        {
            InfoButton.interactable = false;
            return;
        }

        if (!HintUsageManager7.HasShownPopup)
        {
            HintUsageManager7.HasShownPopup = true;
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
        HintUsageManager7.UsageCount++;

        if (HintUsageManager7.UsageCount >= HintUsageManager7.MaxUsage)
        {
            InfoButton.interactable = false;
        }

        SceneManager.LoadScene(hintSceneName);
    }
}
