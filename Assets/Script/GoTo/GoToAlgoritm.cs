using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToAlgoritm : MonoBehaviour
{
    public string AlgoritmScene = "AlgoritmTheoryScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(AlgoritmScene);
        });
    }
}
