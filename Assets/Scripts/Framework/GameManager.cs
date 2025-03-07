using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public static class Settings
{
    [Range(0,1)]
    public static float masterVolume = 1f;
    [Range(0,1)]
    public static float musicVolume = 1f;
    [Range(0,1)]
    public static float sfxVolume = 1f;
    [Range(0,100)]
    public static float cameraSensitivity = 1f;
    public static bool cameraLock = false;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isPaused = false;
    public Image saveIcon;
    public SaveState saveState;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        if(saveState == null)
        {
            saveState = gameObject.AddComponent<SaveState>();
            saveState.name = "profile0";
        }
    }

    public void toggleSavingIcon(bool isSaving)
    {
        if(saveIcon == null) { return; }
        saveIcon.enabled = isSaving;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
