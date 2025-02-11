using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveState : MonoBehaviour
{
    public string profileName;
    private void Start()
    {
        loadSettings("settings");
    }
    public bool load(string profileName)
    {
        try
        {
            saveProfile data = SaveManager.Load(profileName);
            //Reset timescale
            Time.timeScale = 1.0f;
            return true;
        }catch(NullReferenceException e)
        {
            Debug.Log(e.StackTrace);
            return false;
        }
    }
    
    public bool loadSettings(string settingsProfile)
    {
        try
        {
            saveSettings data = SaveManager.LoadSettings(settingsProfile);
            if(data == null) { return true; }
            data.loadSettings();
            //Reset timescale
            Time.timeScale = 1.0f;
            return true;
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e.StackTrace);
            return false;
        }
    }

    public bool save(string profileName)
    {

        //saveProfile data = new saveProfile(profileName, SceneManager.GetActiveScene().buildIndex, ZoneLoader.zoneLoader.roomLoader.curRoom, Movement.getinstance().transform.position, CollectibleList.getInstance().toStringArray(), UpgradeInventory.getInstance().toStringList());
        //SaveManager.Save(data);
        return true;
    }

    public bool saveSettings(string settingName)
    {
        saveSettings settingData = new saveSettings(settingName);
        SaveManager.Save(settingData);
        return true;
    }

    public bool saveSettings()
    {
        return saveSettings("settings");
    }

    public bool save()
    {
        return save("data0");
    }

    public void load()
    {
        load("data0");
    }

    public bool load_confirm()
    {
        return load("data0");
    }
}
