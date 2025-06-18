using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToHigherMathGame : MonoBehaviour
{
    public string higherMath2Scene = "HigherMath1Task";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(higherMath2Scene);
        });
    }
}
