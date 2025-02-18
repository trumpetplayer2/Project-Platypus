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
    public bool getCompleted()
    {
        ItemScript heldItem = PlayerAbilityManager.instance.grab.heldObject;
        if (specificItem != null)
        {
            return specificItem == heldItem;
        }
        if(heldItem != null)
        {
            return heldItem.type == goalType;
        }
        return false;
    }

    public string getStatus()
    {
        return getCompleted() ? "Completed" : "Incomplete";
    }

    public void updateCheck() { }
}
