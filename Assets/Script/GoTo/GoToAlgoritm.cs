using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToAlgoritm : MonoBehaviour
{
    public string AlgoritmScene = "AlgoritmTheoryScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(AlgoritmScene);
        });
    }
}
