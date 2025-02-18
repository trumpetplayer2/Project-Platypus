using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TriggerQuest : QuestTrigger
{
    public QuestTriggerZone[] zones;
    
    

    public bool getCompleted()
    {
        foreach(QuestTriggerZone zone in zones)
        {
            if(!zone.completed) return false;
        }
        return true;
    }

    public string getStatus()
    {
        int count = 0;
        foreach(QuestTriggerZone zone in zones) { if (zone.completed) { count++; } }
        return count + "/" + zones.Length;
    }

    public void updateCheck(){}
}
