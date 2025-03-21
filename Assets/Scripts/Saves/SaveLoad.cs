using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public sealed class saveProfile
{
    public string name;
    public Vector3 lastCheckpoint;
    public int zone = 0;
    //Only stores completed quest. Zone:Index
    public Dictionary<int, int[]> QuestMap;

    private saveProfile() { }

    public saveProfile(string name, Vector3 lastCheckpoint, int lastZone, Dictionary<int, int[]> QuestMap)
    {
        this.name = name;
        this.zone = lastZone;
        this.lastCheckpoint = lastCheckpoint;
        this.QuestMap = QuestMap;
    }
}
[System.Serializable]
public sealed class saveSettings
{
    public string name;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public float cameraSensitivity;
    public bool cameraLock;

    private saveSettings() { }

    public saveSettings(string name, float masterVolume, float musicVolume, float sfxVolume, float cameraSensitivity, bool cameraLock)
    {
        this.name = name;
        this.masterVolume = masterVolume;
        this.musicVolume = musicVolume;
        this.sfxVolume = sfxVolume;
        this.cameraSensitivity = cameraSensitivity;
        this.cameraLock = cameraLock;
    }

    public saveSettings(string name)
    {
        this.name=name;
        this.masterVolume = Settings.masterVolume;
        this.musicVolume = Settings.musicVolume;
        this.sfxVolume = Settings.sfxVolume;
        this.cameraSensitivity = Settings.cameraSensitivity;
        this.cameraLock = Settings.cameraLock;
    }

    public void loadSettings()
    {
        Settings.cameraLock = this.cameraLock;
        Settings.masterVolume = this.masterVolume;
        Settings.musicVolume = this.musicVolume;
        Settings.sfxVolume = this.sfxVolume;
        Settings.cameraSensitivity = this.cameraSensitivity;
    }
}
