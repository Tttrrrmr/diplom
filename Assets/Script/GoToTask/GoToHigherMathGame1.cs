using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToHigherMathGame1 : MonoBehaviour
{
    public string higherMath1Scene = "HigherMath2Task";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(higherMath1Scene);
        });
    }
}
