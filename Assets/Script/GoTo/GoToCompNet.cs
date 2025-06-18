using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToCompNet : MonoBehaviour
{
    public string CompNetScene = "CompNetworksTheoryScene";

    void Start()
    {
        // ������������� ��������� ������� ��� ����� �� ������
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(CompNetScene);
        });
    }
}
