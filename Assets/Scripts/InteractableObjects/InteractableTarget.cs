using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InteractableTarget : ScriptableObject
{
    public string objName;

    public string objDescription;
    
    public float objDuration;

    public bool isActive;

    public bool wasCompleted;

    public bool isPlayer;

    

}
