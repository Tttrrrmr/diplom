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

        // Отключить кнопку, если уже достигнут лимит
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
        HintUsageManager5.UsageCount++;

        if (HintUsageManager5.UsageCount >= HintUsageManager5.MaxUsage)
        {
            InfoButton.interactable = false;
        }

        SceneManager.LoadScene(hintSceneName);
    }
}
