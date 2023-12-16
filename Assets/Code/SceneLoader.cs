using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public static void CloseApp()
    {
# if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
