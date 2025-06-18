using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToSql1 : MonoBehaviour
{
    public string SqlScene = "SQLgame2";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SqlScene);
        });
    }
}
