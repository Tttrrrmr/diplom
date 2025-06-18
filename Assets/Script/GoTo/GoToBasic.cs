using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToBasic : MonoBehaviour
{
    public string BasicScene = "BasicsBDTheoryScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(BasicScene);
        });
    }
}
