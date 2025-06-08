using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI alertText;
    public Button okButton;

    private Action onClose;

    void Awake()
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

    public void Show(string message, Action callback = null)
    {
        alertText.text = message;
        panel.SetActive(true);
        onClose = callback;
    }
}
