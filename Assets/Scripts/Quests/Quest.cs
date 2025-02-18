using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

[System.Serializable]
public class Quest
{
    public string questName = "";
    public string questDescription = "";
    protected int UniqueID;
    [SerializeReference, SubclassSelector]
    public QuestTrigger trigger;
    public int zone = 0;
    public bool hidden = false;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnFinish = new UnityEvent();
    
    
    public void Start()
    {
        OnStart?.Invoke();
    }

    public void Finish()
    {
        OnFinish?.Invoke();
    }

    public void QuestUpdate()
    {
        trigger.updateCheck();
    }

    public string GetQuestName()
    {
        string s = hidden ? "???" : questName;
        return s;
    }

    public string GetQuestDescription()
    {
        return questDescription.Replace("{value}", trigger.getStatus());
    }

    public void RegisterFinishEvent(UnityAction action)
    {
        OnFinish?.AddListener(action);
    }

    public void RegisterStartEvent(UnityAction action)
    {
        OnStart?.AddListener(action);
    }

    public void initializeUID(int uid)
    {
        UniqueID = uid;
    }

    public void unlock()
    {
        hidden = false;
    }
    public int getUID()
    {
        return UniqueID;
    }
}
