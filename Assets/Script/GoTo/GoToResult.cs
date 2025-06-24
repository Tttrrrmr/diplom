using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToResult : MonoBehaviour
{
    public string resultSceneName = "ResultScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(resultSceneName);
        });
    }
}
