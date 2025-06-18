using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToModule2 : MonoBehaviour
{
    public string module1Scene = "ModuleGame2";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(module1Scene);
        });
    }
}
