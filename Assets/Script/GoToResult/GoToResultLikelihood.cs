using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToResultLikelihood : MonoBehaviour
{
    public string resultLikelihoodScene = "ResultLikelihoodScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(resultLikelihoodScene);
        });
    }
}
