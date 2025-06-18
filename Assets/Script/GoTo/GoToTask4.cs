using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTask4 : MonoBehaviour
{
    public string task3SceneName = "TaskScene3";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(task3SceneName);
        });
    }
}
