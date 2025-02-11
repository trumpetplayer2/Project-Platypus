using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseStateClass
{
    GameObject currentTarget;

    public override void StateSetup(AIBase aAIscript)
    {
        this.aiScript = aAIscript;

        //idleTimeUntil = 5;

        Debug.Log("Setting Up idle state State");
    }

    public void SetTarget(GameObject NewTarget)
    {
        currentTarget = NewTarget;
    }

    public override void OnEnterState()
    {

        Debug.Log("In idle State");

        aiScript.agent.destination = currentTarget.transform.position;
        //IsActiveState = true;


        return;
    }

    public override void OnExitState()
    {
        

        //IsActiveState = false;

        Debug.Log("Exiting idle State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState, ref BaseStateClass aCurrState)
    {
        Debug.Log("Changing from Patrol or Idle");
        aCurrState.OnExitState();


        aNewState.OnEnterState();

        return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("interact functionality");

        if(Vector3.Distance(aiScript.gameObject.transform.position, currentTarget.transform.position) < 2)
        {
            aiScript.agent.isStopped = true;

            BeginInteract(currentTarget);
        }

        
        
    }

    private void BeginInteract(GameObject aCurrentTarget)
    {
        Debug.Log("Beginning interacting");

        aiScript.BaseTargetInteractFunction();

        
    }

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
