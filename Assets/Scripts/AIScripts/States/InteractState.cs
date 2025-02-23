using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseStateClass
{

    float timer;

    TargetScript currentTarget;

    InteractableTarget tInfo;
    public InteractState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("In interact State");

        currentTarget = aiScript.RetrieveCurrTarget();

        tInfo = currentTarget.ReturnTargetInfo();

        return;
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting interact State");

        timer = tInfo.objDuration;

        currentTarget = null;

        aiScript.CurrTarget = null;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Changing from Patrol or Idle");

        aNewState.OnEnterState();

        OnExitState();

            return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("interact functionality");

        timer -= Time.deltaTime;

        aiScript.agent.destination = currentTarget.transform.position;

        if (Vector3.Distance(currentTarget.transform.position, aiScript.gameObject.transform.position) < aiScript.distanceBetweenTarget)
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

        Debug.LogFormat("Target Name: {0} , Target Desciption: {1}", tInfo.objName, tInfo.objDescription);

        tInfo.isActive = true;

        if(timer <= 0)
        {
            Debug.Log("task is complete");

            tInfo.wasCompleted = true;

            tInfo.isActive = false;

            aiScript.SwitchStates(aiScript.currActiveState, aiScript.idle);
        }

    }

}
