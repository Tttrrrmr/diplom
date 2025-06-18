using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToBlock2 : MonoBehaviour
{
    public string block1Scene = "BlockScene2";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(block1Scene);
        });
    }
}
