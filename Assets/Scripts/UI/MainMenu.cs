using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void switchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void saveSettings()
    {
        GameManager.instance.saveState.saveSettings();
    }
}
