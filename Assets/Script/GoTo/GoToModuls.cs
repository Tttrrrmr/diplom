using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToModuls : MonoBehaviour
{
    public string ModulsScene = "ModulsTheoryScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(ModulsScene);
        });
    }
}
