using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class SaveManager
{
    private static readonly string saveFolder = Application.persistentDataPath + "/GameData";
    private static readonly string settingFolder = Application.persistentDataPath + "/GameData";

    public static void Delete(string profile)
    {
        if (!File.Exists($"{saveFolder}/{profile}"))
        {
            return;
        }
        File.Delete($"{saveFolder}/{profile}");
    }

    public static saveProfile Load(string profileName)
    {
        if (!File.Exists($"{saveFolder}/{profileName}"))
        {
            throw new System.Exception($"Save Profile {profileName} not found!");
        }

        var fileContents = File.ReadAllText($"{saveFolder}/{profileName}");
        return JsonUtility.FromJson<saveProfile>(fileContents);
    }

    public static saveSettings LoadSettings(string profileName)
    {
        if (!File.Exists($"{settingFolder}/{profileName}"))
        {
            throw new System.Exception($"Save Profile {profileName} not found!");
        }
        var fileContents = File.ReadAllText($"{settingFolder}/{profileName}");
        return JsonUtility.FromJson<saveSettings>(fileContents);
    }

    public static async void Save(saveSettings save)
    {
        GameManager.instance.toggleSavingIcon(true);
        await Task.Run(() =>
        {
            Delete(save.name);
            if (!Directory.Exists(settingFolder))
            {
                Directory.CreateDirectory(settingFolder);
            }
            var JsonString = JsonUtility.ToJson(save);

            File.WriteAllText($"{settingFolder}/{save.name}", JsonString);
        });
        GameManager.instance.toggleSavingIcon(false);
    }

    public static async void Save(saveProfile save)
    {
        GameManager.instance.toggleSavingIcon(true);
        await Task.Run(() =>
        {
            Delete(save.name);
            if (!Directory.Exists(saveFolder))
            {
                Directory.CreateDirectory(saveFolder);
            }
            var JsonString = JsonUtility.ToJson(save);
            File.WriteAllText($"{saveFolder}/{save.name}", JsonString);
        });
        GameManager.instance.toggleSavingIcon(false);
    }
}
