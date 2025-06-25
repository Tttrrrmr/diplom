using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintLikelihoodScript1 : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "Likelihood2Task"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
