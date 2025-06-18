using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GoToCompNet : MonoBehaviour
{
    public string CompNetScene = "CompNetworksTheoryScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(CompNetScene);
        });
    }
}
