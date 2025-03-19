using StateMachineInfo;
using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;

public class AITriggerObj : MonoBehaviour
{

    
    public float radius;

    Collider[] colliders;

    public LayerMask colliderMask;

    int items;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        
    }

    public void CheckAreaForAI()
    {
        Debug.Log("Registered Hit");
        
        items = Physics.OverlapSphereNonAlloc(this.transform.position, radius, colliders, colliderMask);

        for(int i = 0; i < items; i++)
        {
            if (colliders[i].TryGetComponent<AIBase>(out AIBase aI))
            {
                Debug.Log("Found AI");
                aI.playerDetected.TriggerBehavior();
            }

          
        }
    }
}
