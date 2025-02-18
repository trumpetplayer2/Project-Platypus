using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;

[Serializable]
public class QuestTriggerZone : MonoBehaviour
{
    public bool completed;
    public LayerMask layerMask;
    public virtual void OnTriggerEnter(Collider other)
    {
        if (!((layerMask & (1 << other.gameObject.layer)) != 0))
        {
            return;
        }
        completed = true;
    }
}
