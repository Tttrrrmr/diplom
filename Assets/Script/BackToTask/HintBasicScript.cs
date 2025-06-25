using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintBasicScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "AccessGame"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
