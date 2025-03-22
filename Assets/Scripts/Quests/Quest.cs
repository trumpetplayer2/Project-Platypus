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
    public QuestUIContainer container;
    protected int UniqueID;
    [SerializeReference, SubclassSelector]
    public QuestTrigger trigger;
    public int zone = 0;
    public bool hidden = false;
    bool finished = false;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnFinish = new UnityEvent();
    float descriptionCheckCooldown = 0f;
    public float descriptionUpdate = 1f;
    
    
    public void Start()
    {
        OnStart?.Invoke();
        container.updateQuestName(GetQuestName());
        updateQuestDescription();
    }

    public void Finish()
    {
        OnFinish?.Invoke();
        QuestManager.instance.saveQuestData();
    }

    public void QuestUpdate()
    {
        if (!hidden)
        {
            trigger.updateCheck();
            descriptionCheckCooldown += Time.deltaTime;
            if(descriptionCheckCooldown >= descriptionUpdate)
            {
                updateQuestDescription();
                descriptionCheckCooldown = 0;
            }
            if (trigger.getCompleted() && !finished)
            {
                finished = true;
                Finish();
            }
        }
    }

    public void updateQuestDescription()
    {
        if (!hidden)
        {
            container?.updateQuestDescription(GetQuestDescription());
        }
        else
        {
            container.updateQuestDescription("???");
        }
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
        Start();
    }
    public int getUID()
    {
        return UniqueID;
    }

    public void forceComplete()
    {
        trigger.forceComplete();
    }

    public bool getCompleted()
    {
        return finished;
    }
}
