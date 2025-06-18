using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToModule2 : MonoBehaviour
{
    public string module1Scene = "ModuleGame2";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(module1Scene);
        });
    }
}
