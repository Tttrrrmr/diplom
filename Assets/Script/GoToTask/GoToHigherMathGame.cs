using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToHigherMathGame : MonoBehaviour
{
    public string higherMath2Scene = "HigherMath1Task";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(higherMath2Scene);
        });
    }
}
