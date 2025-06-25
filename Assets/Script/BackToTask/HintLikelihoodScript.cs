using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintLikelihoodScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "Likelihood1Task"; // Название сцены с заданием

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
