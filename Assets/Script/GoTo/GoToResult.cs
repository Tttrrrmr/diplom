using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToResult : MonoBehaviour
{
    public string resultSceneName = "ResultScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(resultSceneName);
        });
    }
}
