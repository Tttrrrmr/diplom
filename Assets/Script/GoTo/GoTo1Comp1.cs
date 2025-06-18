using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoTo1Comp1 : MonoBehaviour
{
    public string comp1Scene1 = "SystemScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(comp1Scene1);
        });
    }
}
