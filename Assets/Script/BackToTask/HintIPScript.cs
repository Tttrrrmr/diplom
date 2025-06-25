using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintIPScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "CompNetworkTask"; // Название сцены с заданием

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
