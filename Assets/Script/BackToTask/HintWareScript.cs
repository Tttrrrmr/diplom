using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintWareScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "WireGamee"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
