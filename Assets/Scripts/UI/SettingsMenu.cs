using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public Slider masterVol;
    public Slider musicVol;
    public Slider sfxVol;
    public Slider cameraSensitivity;
    public Toggle cameraLock;
    public Submenu otherMenu;
    public Submenu thisMenu;
    float[] previousValues = new float[4];
    bool previousLock = false;
    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if(otherMenu != null && thisMenu != null)
            {
                revert();
                thisMenu.togglePauseMenu(otherMenu);
            }
        }
    }
    public void refresh()
    {
        masterVol.value = Settings.masterVolume;
        musicVol.value = Settings.musicVolume;
        sfxVol.value = Settings.sfxVolume;
        cameraSensitivity.value = Settings.cameraSensitivity;
        cameraLock.isOn = Settings.cameraLock;

        previousValues = new float[] { masterVol.value, musicVol.value, sfxVol.value, cameraSensitivity.value };
        previousLock = cameraLock.isOn;
    }

    public void revert()
    {
        Settings.masterVolume = previousValues[0];
        Settings.musicVolume = previousValues[1];
        Settings.sfxVolume = previousValues[2];
        Settings.cameraSensitivity = previousValues[3];
        Settings.cameraLock = previousLock;
    }

    public void masterVolumeChange(float value)
    {
        Settings.masterVolume = value;
    }

    public void musicVolumeChange(float value)
    {
        Settings.musicVolume = value;
    }

    public void sfxVolumeChange(float value)
    {
        Settings.sfxVolume = value;
    }

    public void cameraSensitivityChange(float value)
    {
        Settings.cameraSensitivity = value;
    }

    public void cameraLockChange(bool value)
    {
        Settings.cameraLock = value;
    }

    public void save()
    {
        GameManager.instance.saveState.saveSettings();
    }
}
