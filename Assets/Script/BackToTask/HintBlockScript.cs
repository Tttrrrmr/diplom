using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintBlockScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "BlockScene1"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
