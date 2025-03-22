using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using tp2;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public GameObject[] hideOnPause;
    public GameObject[] showOnPause;
    public Image fade;
    bool fading = false;
    public float fadeTime = .5f;
    float fadeCounter = 0;
    public Vector3 checkpoint;
    bool canTeleport = false;
    public Submenu currentMenu;
    public static PauseScript instance;

    private void Start()
    {
        instance = this;
        GameManager.instance.isPaused = false;
        if(GameManager.instance.loadingIn)
        {
            checkpoint = GameManager.instance.loadCheckpoint;
            GameManager.instance.loadingIn = false;
            returnToCheckpoint();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel")){
            if (currentMenu.layer < 1)
            {
                togglePause();
            }
        }
        Fade();
        if (canTeleport)
        {
            PlayerAbilityManager.instance.Release();
            tp2.PlayerMovement.instance.warp(checkpoint);
        }
    }



    public void togglePause()
    {

        //Hide UI
        foreach (GameObject obj in hideOnPause)
        {
            if (obj != null)
            {
                obj.SetActive(GameManager.instance.isPaused);
            }
        }
        foreach (GameObject obj in showOnPause)
        {
            if (obj != null)
            {
                obj.SetActive(!GameManager.instance.isPaused);
            }
        }
        Time.timeScale = GameManager.instance.isPaused ? 1 : 0;
        Cursor.lockState = GameManager.instance.isPaused ? CursorLockMode.Locked : CursorLockMode.None;
        GameManager.instance.isPaused = !GameManager.instance.isPaused;
    }

    public void returnToCheckpoint()
    {
        if (GameManager.instance.isPaused) { togglePause(); }
        fading = true;
        fadeCounter = 0;
    }

    public void Fade()
    {
        if(fading)
        {
            fadeCounter += Time.deltaTime;
            if (fadeCounter < fadeTime)
            {
                Color c = fade.color;
                c.a = fadeCounter/fadeTime;
                fade.color = c;
            }else if(fadeCounter < fadeTime * 2)
            {
                canTeleport = true;
                Color c = fade.color;
                c.a = (2 * fadeTime - fadeCounter)/fadeTime;
                fade.color = c;
            }
            else
            {
                canTeleport = false;
                fading = false;
            }
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
