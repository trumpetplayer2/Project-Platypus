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
        colliders = new Collider[3];
    }

    public void CheckAreaForAI()
    {
        Debug.Log("Registered Hit");
        
        items = Physics.OverlapSphereNonAlloc(this.transform.position, radius, colliders, colliderMask);

        Debug.Log("Overlap Sphere activated");

        for(int i = 0; i < items; i++)
        {
            Debug.Log("Am I in For loop");

            if (colliders[i].TryGetComponent<AIBase>(out AIBase aI))
            {
                Debug.Log("Found AI");
                aI.TriggerBehavior();
            }

          
        }
    }
}
