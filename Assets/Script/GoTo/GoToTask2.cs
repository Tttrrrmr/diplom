using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTask2 : MonoBehaviour
{
    public string task2SceneName = "TaskScene1";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(task2SceneName);
        });
    }
}
