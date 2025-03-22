using System.Collections.Generic;
using System.Threading.Tasks;
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
    public Button continueButton;
    public Image saveIcon;
    public SaveState saveState;
    public Dictionary<int, int[]> QuestMap = new Dictionary<int, int[]>();
    public bool reset = false;
    public Vector3 loadCheckpoint = Vector3.zero;
    public bool loadingIn = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if (!instance.reset)
            {
                //Pass Quest Map
                QuestMap = instance.QuestMap;
                loadCheckpoint = instance.loadCheckpoint;
                loadingIn = instance.loadingIn;
            }
            Destroy(instance.gameObject);
            instance = this;
        }
        if(saveState == null)
        {
            saveState = gameObject.AddComponent<SaveState>();
            saveState.profileName = "profile0";
            saveState.loadSettings("settings");
        }
        if (continueButton != null) continueButton.interactable = (canLoad());
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

    public void load()
    {
        if (!canLoad()) return;
        reset = false;
        saveState.load();
    }

    public void save()
    {
        if (saveState == null) Debug.Log("Save state was null");
        saveState.save();
    }

    public bool canLoad()
    {
        return SaveManager.canLoad(saveState.profileName);
    }

    public void updateQuestData()
    {
        updateQuestData(QuestManager.instance.getQuestData());
    }

    void updateQuestData(KeyValuePair<int, int[]> pair)
    {
        QuestMap.Add(pair.Key, pair.Value);
    }
}
