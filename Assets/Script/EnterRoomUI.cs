using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnterRoomUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI titleText;

    private string sceneToLoad;
    private Action onClose;

    public void Show(string scene, string room)
    {
        sceneToLoad = scene;
        titleText.text = $"Вы хотите зайти в {room}?";
        panel.SetActive(true);
    }

    public void OnYesClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnCancelClicked()
    {
        panel.SetActive(false);
        onClose?.Invoke();
        onClose = null;
    }

    public void SetOnClose(Action callback)
    {
        onClose = callback;
    }
}

