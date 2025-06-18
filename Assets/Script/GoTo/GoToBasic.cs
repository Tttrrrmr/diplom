using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToBasic : MonoBehaviour
{
    public string BasicScene = "BasicsBDTheoryScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(BasicScene);
        });
    }
}
