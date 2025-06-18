using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToPaper : MonoBehaviour
{
    public string paperScene = "MathScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(paperScene);
        });
    }
}
