using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTask3 : MonoBehaviour
{
    public string task2SceneName = "TaskScene2";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(task2SceneName);
        });
    }
}
