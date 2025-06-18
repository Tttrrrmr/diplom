using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToLikelihoodGame : MonoBehaviour
{
    public string likelihoodGameScene = "Likelihood1Task";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(likelihoodGameScene);
        });
    }
}
