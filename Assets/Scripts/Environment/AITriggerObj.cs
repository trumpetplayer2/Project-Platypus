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

    private void Awake()
    {
        
    }

    public void CheckAreaForAI()
    {
        items = Physics.OverlapSphereNonAlloc(this.transform.position, radius, colliders, colliderMask);

        for(int i = 0; i < items; i++)
        {
            if (colliders[i].TryGetComponent<AIBase>(out AIBase aI))
            {
                aI.playerDetected.TriggerBehavior();

              
            }

            //don't like doing this, there has to be a better way
            if (colliders[i].CompareTag("Player") && colliders[i].TryGetComponent<TargetScript>(out TargetScript aTarget))
            {
                aI.searchFunctionSettings.playerObj = aTarget;
            }

        }
    }
}
