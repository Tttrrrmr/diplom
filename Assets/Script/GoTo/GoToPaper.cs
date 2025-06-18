using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToPaper : MonoBehaviour
{
    public string paperScene = "MathScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(paperScene);
        });
    }
}
