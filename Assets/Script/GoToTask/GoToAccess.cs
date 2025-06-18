using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToAccess : MonoBehaviour
{
    public string accessScene = "AccessGame";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(accessScene);
        });
    }
}
