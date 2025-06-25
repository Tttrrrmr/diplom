using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintBlockScript1 : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "BlockScene2"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
