using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoTo1Comp : MonoBehaviour
{
    public string comp1Scene = "AdminScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(comp1Scene);
        });
    }
}
