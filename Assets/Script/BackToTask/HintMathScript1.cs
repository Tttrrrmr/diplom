using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintMathScript1 : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "HigherMath2Task"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
