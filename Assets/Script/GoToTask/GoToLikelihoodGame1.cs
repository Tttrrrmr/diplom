using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToLikelihoodGame1 : MonoBehaviour
{
    public string likelihood1GameScene = "Likelihood2Task";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(likelihood1GameScene);
        });
    }
}
