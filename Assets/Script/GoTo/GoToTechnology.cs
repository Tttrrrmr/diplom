using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTechnology : MonoBehaviour
{
    public string TechnologyScene = "TechnologyBDTheoryScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(TechnologyScene);
        });
    }
}
