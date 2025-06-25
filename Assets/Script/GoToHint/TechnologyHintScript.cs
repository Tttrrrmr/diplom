using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TechnologyHintScript : MonoBehaviour
{
    [Header("UI Elements")]
    public Button InfoButton;
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button okButton;

    [Header("Scene Settings")]
    public string hintSceneName = "TechnologyHintScene";

    private void Start()
    {
        popupPanel.SetActive(false);
        InfoButton.onClick.AddListener(OnHintButtonClicked);
        okButton.onClick.AddListener(OnOkClicked);

        // Отключить кнопку, если уже достигнут лимит
        if (HintUsageManager11.UsageCount >= HintUsageManager11.MaxUsage)
        {
            InfoButton.interactable = false;
        }
    }

    private void OnHintButtonClicked()
    {
        if (HintUsageManager11.UsageCount >= HintUsageManager11.MaxUsage)
        {
            InfoButton.interactable = false;
            return;
        }

        if (!HintUsageManager11.HasShownPopup)
        {
            HintUsageManager11.HasShownPopup = true;
            popupPanel.SetActive(true);
            popupText.text = "Вы можете воспользоваться подсказкой не более 3 раз.";
            return; // Ждём нажатия кнопки "ОК"
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
        HintUsageManager11.UsageCount++;

        if (HintUsageManager11.UsageCount >= HintUsageManager11.MaxUsage)
        {
            InfoButton.interactable = false;
        }

        SceneManager.LoadScene(hintSceneName);
    }
}
