using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerZone : QuestTriggerZone
{
    public ItemType GoalType;
    public override void OnTriggerEnter(Collider other)
    {
        if (!((layerMask & (1 << other.gameObject.layer)) != 0))
        {
            return;
        }
        if(!other.TryGetComponent<ItemScript>(out ItemScript item)) { return; }
        if(item.type != GoalType) { return; }
        completed = true;
    }
}
