using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ManualTriggerQuest : QuestTrigger
{
    bool[] completionCheck;

    public void completeCheck(int i)
    {
        completionCheck[i] = true;
    }

    public bool getCompleted()
    {
        foreach(bool b in completionCheck)
        {
            if (!b)
            {
                return false;
            }
        }
        return true;
    }

    public string getStatus()
    {
        int i = 0;
        foreach (bool b in completionCheck)
        {
            if (b) i++;
        }
        return i + "/" + completionCheck.Length;
    }

    public void updateCheck()
    {
        
    }

    public void forceComplete()
    {
        for(int i = 0; i < completionCheck.Length; i++)
        {
            completionCheck[i] = true;
        }
    }
}
