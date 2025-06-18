using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToBlock1 : MonoBehaviour
{
    public string blockScene = "BlockScene1";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(blockScene);
        });
    }
}
