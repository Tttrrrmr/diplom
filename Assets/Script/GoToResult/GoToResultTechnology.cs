using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToResultTechnology : MonoBehaviour
{
    public string resultTechnologyScene = "ResultTechnologyScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(resultTechnologyScene);
        });
    }
}
