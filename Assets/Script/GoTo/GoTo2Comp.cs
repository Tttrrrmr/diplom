using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoTo2Comp : MonoBehaviour
{
    public string comp2Scene = "ProgScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(comp2Scene);
        });
    }
}
