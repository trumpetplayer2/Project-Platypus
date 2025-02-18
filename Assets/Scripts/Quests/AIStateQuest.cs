using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIStateQuest : QuestTrigger
{
    public AIBase AI;
    [SerializeField]
    public string goalStateType;
    bool completed = false;
    public bool getCompleted()
    {
        return completed;
    }

    public string getStatus()
    {
        return getCompleted() ? "Completed" : "Incomplete";
    }

    public void updateCheck()
    {
        if (AI.currActiveState.GetType().ToString().Equals(goalStateType))
        {
            completed = true;
        }
    }
}
