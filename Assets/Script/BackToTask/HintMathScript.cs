using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintMathScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "HigherMath1Task"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
