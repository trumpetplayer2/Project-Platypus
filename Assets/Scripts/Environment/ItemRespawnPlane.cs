using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawnPlane : MonoBehaviour
{
    public bool AllItems = true;
    
    public ItemScript[] SpecificItems;
    public ItemType SpecificType;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<ItemScript>(out ItemScript item)) return;
        if (AllItems)
        {
            item.respawn();
            return;
        }
        foreach(ItemScript i in SpecificItems)
        {
            if (!(item.transform == i.transform)) continue;
            item.respawn();
            return;
        }
        if(item.type == SpecificType)
        {
            item.respawn();
            return;
        }
    }
}
