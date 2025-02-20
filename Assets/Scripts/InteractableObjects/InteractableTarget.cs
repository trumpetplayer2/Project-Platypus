using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
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

    public enum AIResponse
    {
        Chase,
        StopLook,
        
    }

    public AIResponse SetResponse;

}
