using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToAccess : MonoBehaviour
{
    public string accessScene = "AccessGame";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(accessScene);
        });
    }
}
