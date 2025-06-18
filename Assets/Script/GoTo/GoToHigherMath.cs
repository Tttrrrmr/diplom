using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToHigherMath : MonoBehaviour
{
    public string HigherMathScene = "HigherMathematicsTheory";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(HigherMathScene);
        });
    }
}
