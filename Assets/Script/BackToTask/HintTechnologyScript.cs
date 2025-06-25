using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintTechnologyScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "SQLgame"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
