using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoTo2Comp1 : MonoBehaviour
{
    public string comp2Scene1 = "BdScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(comp2Scene1);
        });
    }
}
