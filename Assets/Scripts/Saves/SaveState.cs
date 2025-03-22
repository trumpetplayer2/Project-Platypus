using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveState : MonoBehaviour
{
    public string profileName = "PROFILENAMENOTFOUND";

    async Task<bool> load(string profileName)
    {
        try
        {
            saveProfile data = SaveManager.Load(profileName);
            //Update Quest Map
            GameManager.instance.QuestMap = data.QuestMap;
            //This may cause an error, we'll need to see
            GameManager.instance.loadCheckpoint = data.lastCheckpoint;
            GameManager.instance.loadingIn = true;

            SceneManager.LoadScene(data.zone);

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
            saveSettings();
            return false;
        }
    }

    public bool save(string profileName)
    {

        //saveProfile data = new saveProfile(profileName, SceneManager.GetActiveScene().buildIndex, ZoneLoader.zoneLoader.roomLoader.curRoom, Movement.getinstance().transform.position, CollectibleList.getInstance().toStringArray(), UpgradeInventory.getInstance().toStringList());
        //SaveManager.Save(data);
        saveProfile data = new saveProfile(profileName, PauseScript.instance.checkpoint, SceneManager.GetActiveScene().buildIndex, GameManager.instance.QuestMap);
        SaveManager.Save(data);
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
        return save(profileName);
    }

    public void load()
    {
        StartCoroutine("load", profileName);
    }
}
