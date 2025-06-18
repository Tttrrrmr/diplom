using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToModule1 : MonoBehaviour
{
    public string moduleScene = "ModuleGame";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(moduleScene);
        });
    }
}
