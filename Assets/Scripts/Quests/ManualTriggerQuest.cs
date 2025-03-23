using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ManualTriggerQuest : QuestTrigger
{
    bool completed = false;

    public bool getCompleted()
    {
        return completed;
    }

    public string getStatus()
    {
        return completed ? "Complete" : "Incomplete";
    }

    public void updateCheck()
    {
        
    }

    public void forceComplete()
    {
        completed = true;
    }
}
