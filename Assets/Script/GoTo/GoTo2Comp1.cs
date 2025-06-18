using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoTo2Comp1 : MonoBehaviour
{
    public string comp2Scene1 = "BdScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(comp2Scene1);
        });
    }
}
