using System;
using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;

[Serializable]
public class ItemQuest : QuestTrigger
{

    public ItemType goalType;
    public ItemScript specificItem;
    bool completed = false;
    public bool getCompleted()
    {
        if (completed) return true;
        if (PlayerAbilityManager.instance == null) return false;
        ItemScript heldItem = PlayerAbilityManager.instance.grab.heldObject;
        if (specificItem != null)
        {
            completed = specificItem == heldItem;
            return specificItem == heldItem;
        }
        if(heldItem != null)
        {
            completed = heldItem.type == goalType;
            return heldItem.type == goalType;
        }

        return false;
    }

    public string getStatus()
    {
        return getCompleted() ? "Completed" : "Incomplete";
    }

    public void updateCheck() { }

    public void forceComplete()
    {
        completed = true;
    }
}
