using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToWire : MonoBehaviour
{
    public string wireScene = "WireGamee";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(wireScene);
        });
    }
}
