using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoToResultProg : MonoBehaviour
{
    public string resultProgScene = "ResultProgScene";

    void Start()
    {
        // Автоматически добавляем событие при клике на кнопку
        GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(resultProgScene);
        });
    }
}
