using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AlertUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI alertText;
    public Button okButton;

    private Action onClose;

    void Start()
    {
        panel.SetActive(false);
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            onClose?.Invoke();
        });
    }

    public void Show(string message, Action callback = null)
    {
        alertText.text = message;
        panel.SetActive(true);
        onClose = callback;
    }
}
