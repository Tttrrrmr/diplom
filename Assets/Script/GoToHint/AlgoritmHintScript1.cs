using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AlgoritmHintScript1 : MonoBehaviour
{
    [Header("UI Elements")]
    public Button InfoButton;
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button okButton;

    [Header("Scene Settings")]
    public string hintSceneName = "AlgoritmHintScene1";

    private void Start()
    {
        popupPanel.SetActive(false);
        InfoButton.onClick.AddListener(OnHintButtonClicked);
        okButton.onClick.AddListener(OnOkClicked);

        // Отключить кнопку, если уже достигнут лимит
        if (HintUsageManager1.UsageCount >= HintUsageManager1.MaxUsage)
        {
            InfoButton.interactable = false;
        }
    }

    private void OnHintButtonClicked()
    {
        if (HintUsageManager1.UsageCount >= HintUsageManager1.MaxUsage)
        {
            InfoButton.interactable = false;
            return;
        }

        if (!HintUsageManager1.HasShownPopup)
        {
            HintUsageManager1.HasShownPopup = true;
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
        HintUsageManager1.UsageCount++;

        if (HintUsageManager1.UsageCount >= HintUsageManager1.MaxUsage)
        {
            InfoButton.interactable = false;
        }

        SceneManager.LoadScene(hintSceneName);
    }
}
