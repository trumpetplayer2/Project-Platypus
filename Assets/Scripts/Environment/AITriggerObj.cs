using StateMachineInfo;
using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;

/// <summary>
/// An object that, upon being hit by gravel from the player, will create an overlap sphere, that, if an AI is within, will activate the trigger behavior
/// </summary>
public class AITriggerObj : MonoBehaviour
{
    public float radius;

    Collider[] colliders;

    public LayerMask colliderMask;

    int items;

    private void Awake()
    {
        colliders = new Collider[3];
    }


    public void CheckAreaForAI()
    {
        
        
        items = Physics.OverlapSphereNonAlloc(this.transform.position, radius, colliders, colliderMask);

        

        for(int i = 0; i < items; i++)
        {
           

            if (colliders[i].TryGetComponent<AIBase>(out AIBase aI))
            {
               
                aI.TriggerBehavior();
            }

          
        }
    }
}
