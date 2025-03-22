using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

[System.Serializable]
public sealed class saveProfile
{
    public string name;
    public Vector3 lastCheckpoint;
    public int zone = 0;
    //Only stores completed quest. Zone:Index
    public byte[] QuestMap;

    private saveProfile() { }

    public saveProfile(string name, Vector3 lastCheckpoint, int lastZone, Dictionary<int, int[]> map)
    {
        this.name = name;
        this.zone = lastZone;
        this.lastCheckpoint = lastCheckpoint;
        QuestMap = Serialize(map);
    }

    public byte[] Serialize(System.Object obj)
    {
        if (obj == null)
        {
            return null;
        }

        using (var memoryStream = new MemoryStream())
        {
            var binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
    }
    
    public Dictionary<int, int[]> getQuestMap()
    {
        return (Dictionary<int, int[]>) Deserialize(QuestMap);
    }

    static System.Object Deserialize(byte[] arrBytes)
    {
        using (var memoryStream = new MemoryStream())
        {
            var binaryFormatter = new BinaryFormatter();

            memoryStream.Write(arrBytes, 0, arrBytes.Length);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return binaryFormatter.Deserialize(memoryStream);
        }
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
