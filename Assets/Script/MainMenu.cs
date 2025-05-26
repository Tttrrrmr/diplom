using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("AuthScene");
    }

    public void RegGame()
    {
        SceneManager.LoadScene("RegScene");
    }

    public void ExitGame()
    {
        Debug.Log("Приложение закрылось");
        Application.Quit();
    }
}
