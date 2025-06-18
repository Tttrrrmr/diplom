using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToHigherMath : MonoBehaviour
{
    public string HigherMathScene = "HigherMathematicsTheory";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(HigherMathScene);
        });
    }
}
