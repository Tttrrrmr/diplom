using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToCorridor : MonoBehaviour
{
    public string corridorSceneName = "MainScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(corridorSceneName);
        });
    }
}
