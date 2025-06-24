using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToResultModule : MonoBehaviour
{
    public string resultModuleScene = "ResultModuleScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(resultModuleScene);
        });
    }
}
