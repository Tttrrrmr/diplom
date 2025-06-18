using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTechnology : MonoBehaviour
{
    public string TechnologyScene = "TechnologyBDTheoryScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(TechnologyScene);
        });
    }
}
