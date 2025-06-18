using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToLikelihood : MonoBehaviour
{
    public string LikelihoodScene = "LikelihoodTheoryScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(LikelihoodScene);
        });
    }
}
