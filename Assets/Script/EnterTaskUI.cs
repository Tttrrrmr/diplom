using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class EnterTaskUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public Button yesButton;
    public Button cancelButton;

    private string sceneToLoad;
    private Action closeCallback;

    void Start()
    {
        panel.SetActive(false);

        yesButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneToLoad);
        });

        cancelButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            closeCallback?.Invoke();
        });
    }

    public void Show(string taskName, string sceneName, Action onClose)
    {
        titleText.text = $"Вы хотите перейти к заданию \"{taskName}\"?";
        sceneToLoad = sceneName;
        closeCallback = onClose;
        panel.SetActive(true);
    }
}


