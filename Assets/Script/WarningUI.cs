using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WarningUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI text;
    public Button okButton;

    private Action onClose;

    private void Awake()
    {
        panel.SetActive(false);

        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            onClose?.Invoke();
            onClose = null;
        });
    }

    public void Show(string message, Action onCloseCallback)
    {
        text.text = message;
        panel.SetActive(true);
        onClose = onCloseCallback;
    }
}




