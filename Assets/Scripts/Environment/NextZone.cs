using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextZone : MonoBehaviour
{
    public int scene;
    bool switching = false;
    private void OnTriggerEnter(Collider other)
    {
        if (switching) return;
        if (!other.tag.ToLower().Equals("player")) return;
        if (Fade.instance != null)
        {
            Invoke("loadScene", Fade.instance.fadeTime);
            Fade.instance.fadeDirection = true;
            Fade.instance.fading = true;
        }
        else
        {
            loadScene();
        }
        switching = true;
    }

    void loadScene()
    {
        SceneManager.LoadScene(scene);
    }
}
