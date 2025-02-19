using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseStateClass
{
    public InteractState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    TargetScript currentTarget;

    public override void OnEnterState()
    {

        Debug.Log("In interact State");
        currentTarget = aiScript.RetrieveCurrTarget();
       // aiScript.agent.isStopped = false;
        aiScript.agent.destination = currentTarget.transform.position;

        Debug.Log("Should have AI moving to target");
        return;
    }

    public override void OnExitState()
    {
        
        currentTarget = null;

        aiScript.CurrTarget = null;

        Debug.Log("Exiting interact State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Changing from Patrol or Idle");

        OnExitState();

            return;
        
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("interact functionality");

        if (Vector3.Distance(currentTarget.transform.position, aiScript.gameObject.transform.position) < aiScript.agent.stoppingDistance)
        {
            Debug.Log("AI is stopped in front of target");
            aiScript.agent.isStopped = true;
            BeginInteract(currentTarget);
        }

        return;
        
    }

    private void BeginInteract(TargetScript aCurrentTarget)
    {
        Debug.Log("Beginning interacting");

        InteractableTarget tInfo = aCurrentTarget.ReturnTargetInfo();

        Debug.LogFormat("Target Name: {0} , Target Desciption: {1}", tInfo.objName, tInfo.objDescription);

        float interactTimer = tInfo.objDuration;
    
        interactTimer -= Time.deltaTime + 5;

        tInfo.isActive = true;

        Debug.Log(interactTimer);

        if(interactTimer <= 0)
        {
            Debug.Log("task is complete");

            tInfo.wasCompleted = true;

            tInfo.isActive = false;

            aiScript.SwitchStates(aiScript.currActiveState, aiScript.idle);
        }

    

    }

   
 

}
