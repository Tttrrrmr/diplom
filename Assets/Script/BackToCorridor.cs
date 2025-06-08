using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToCorridor : MonoBehaviour
{
    public string corridorSceneName = "MainScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(corridorSceneName);
        });
    }
}
