using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToIP : MonoBehaviour
{
    public string IPScene = "CompNetworkTask";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(IPScene);
        });
    }
}
