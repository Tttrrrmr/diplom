using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToTask : MonoBehaviour
{
    public string taskSceneName = "TaskScene";

    void Start()
    {
        // �������� ��������� Button � ��������� ���������� �����
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(LoadTaskScene);
        }
        else
        {
            Debug.LogError("GoToTask script requires a Button component!");
        }
    }

    void LoadTaskScene()
    {
        // ���������, ���������� �� ����� ����� ���������
        if (Application.CanStreamedLevelBeLoaded(taskSceneName))
        {
            SceneManager.LoadScene(taskSceneName);
        }
        else
        {
            Debug.LogError($"Scene {taskSceneName} cannot be loaded! Make sure it's added to Build Settings.");
        }
    }
}
