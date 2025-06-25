using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HintModuleScript : MonoBehaviour
{
    public Button closeButton;
    public string returnSceneName = "ModuleGame"; // �������� ����� � ��������

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(returnSceneName);
        });
    }
}
