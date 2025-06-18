using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToSql : MonoBehaviour
{
    public string SQLScene = "SQLgame";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SQLScene);
        });
    }
}
