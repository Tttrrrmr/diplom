using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintLikelihoodScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "Likelihood1Task"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
