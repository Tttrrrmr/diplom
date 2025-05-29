using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EnterRoomUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI titleText;

    private string sceneToLoad;

    public event Action OnClose;

    public void Show(string scene, string room)
    {
        sceneToLoad = scene;
        titleText.text = $"Вы хотите зайти в {room}?";
        panel.SetActive(true);
    }

    public void OnYesClicked()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnCancelClicked()
    {
        panel.SetActive(false);
        OnClose?.Invoke(); // оповещаем дверь
        OnClose = null;     // очищаем старую подписку
    }
}
