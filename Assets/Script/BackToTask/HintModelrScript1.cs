using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintModelrScript1 : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "ModuleGame2"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
