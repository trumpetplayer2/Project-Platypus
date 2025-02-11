using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public sealed class saveProfile
{
    public string name;
    public Vector3 lastCheckpoint;


    private saveProfile() { }

    public saveProfile(string name)
    {
        this.name = name;
        
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
