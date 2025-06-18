using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToModule1 : MonoBehaviour
{
    public string moduleScene = "ModuleGame";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(moduleScene);
        });
    }
}
