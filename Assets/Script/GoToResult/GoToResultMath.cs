using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToResultMath : MonoBehaviour
{
    public string resultMathScene = "ResultMathScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(resultMathScene);
        });
    }
}
