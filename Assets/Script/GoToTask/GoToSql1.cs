using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToSql1 : MonoBehaviour
{
    public string SqlScene = "SQLgame2";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SqlScene);
        });
    }
}
