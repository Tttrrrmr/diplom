using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToResultSystem : MonoBehaviour
{
    public string resultSystemScene = "ResultSystemScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(resultSystemScene);
        });
    }
}
