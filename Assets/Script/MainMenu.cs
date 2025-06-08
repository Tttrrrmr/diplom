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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Для редактора
#else
    Application.Quit(); // Для билда
#endif
    }

}
