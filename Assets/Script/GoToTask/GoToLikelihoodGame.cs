using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToLikelihoodGame : MonoBehaviour
{
    public string likelihoodGameScene = "Likelihood1Task";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(likelihoodGameScene);
        });
    }
}
