using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToLikelihood : MonoBehaviour
{
    public string LikelihoodScene = "LikelihoodTheoryScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(LikelihoodScene);
        });
    }
}
