using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TriggerQuest : QuestTrigger
{
    public QuestTriggerZone[] zones;
    bool completed = false;
    

    public bool getCompleted()
    {
        if (completed) return true;
        foreach(QuestTriggerZone zone in zones)
        {
            if(!zone.completed) return false;
        }
        completed = true;
        return true;
    }

    public string getStatus()
    {
        int count = 0;
        foreach(QuestTriggerZone zone in zones) { if (zone.completed) { count++; } }
        return count + "/" + zones.Length;
    }

    public void updateCheck(){}

    public void forceComplete()
    {
        foreach(QuestTriggerZone zone in zones)
        {
            zone.completed = true;
        }
        completed = true;
    }
}
