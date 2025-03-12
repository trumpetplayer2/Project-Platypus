using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseStateClass
{
    float timer;

    TargetScript currentTarget;

    InteractableTarget tInfo;

    public InteractState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("In interact State");

        currentTarget = aiScript.searchFunctionSettings.CurrTarget == null ? null : aiScript.searchFunctionSettings.CurrTarget;

        tInfo = currentTarget.ReturnTargetInfo();

        timer = currentTarget.TargetInfo.objDuration;

        aiScript.agent.isStopped = false;

        return;
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting interact State");

        currentTarget = null;

        aiScript.searchFunctionSettings.CurrTarget = null;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        //Debug.Log("Changing from Patrol or Idle");

        aNewState.OnEnterState();

        OnExitState();

            return;
    }

    public override void CurrStateFunctionality()
    {
        //Debug.Log("interact functionality");
        if(aiScript.SearchForTargets() == DetectedType.Player)
        {
            aiScript.SwitchStates(aiScript.playerDetected);
            return;
        }

        timer -= Time.deltaTime;

        aiScript.agent.destination = currentTarget.transform.position;

        if (Vector3.Distance(currentTarget.transform.position, aiScript.gameObject.transform.position) < aiScript.interactSettings.distanceBetweenTarget)
        {
            Debug.Log("AI is stopped in front of target");
            aiScript.agent.isStopped = true;
            BeginInteract(currentTarget);
            return;
        }

        return;
        
    }

 
    private void BeginInteract(TargetScript aCurrentTarget)
    {
        Debug.Log("Beginning interacting");

        Debug.LogFormat("Target Name: {0} , Target Desciption: {1}", tInfo.objName, tInfo.objDescription);

        currentTarget.TargetInfo.isActive = true;

        if(timer <= 0)
        {
            Debug.Log("task is complete");

            currentTarget.TargetInfo.wasCompleted = true;

            currentTarget.TargetInfo.isActive = false;

            aiScript.SwitchStates(aiScript.idle);

            return;
        }

        return;

    }

}
