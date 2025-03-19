using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Rendering;
using UnityEngine;

public class ChaseState : BaseStateClass
{
    public TargetScript chasingTarget;

    float losingTimer;

    float catchTimer;

    public ChaseState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Chase State");
        chasingTarget = aiScript.searchFunctionSettings.playerObj;

        aiScript.agent.speed += aiScript.chaseSettings.chaseSpeedIncrease;

        losingTimer = aiScript.chaseSettings.losingTargetTime;

        catchTimer = aiScript.chaseSettings.catchTimerTimer;

        aiScript.agent.isStopped = false;

        return;
    }

    public override void CurrStateFunctionality()
    {
            aiScript.agent.destination = chasingTarget.transform.position;

            if (Vector3.Distance(aiScript.searchFunctionSettings.Eyes.gameObject.transform.position, chasingTarget.transform.position) > aiScript.chaseSettings.chaseMaxDistance)
            {
                Debug.Log("Calling Losing Target");

                LosingTarget();

                return;
            }


            if (Vector3.Distance(aiScript.searchFunctionSettings.Eyes.gameObject.transform.position, chasingTarget.transform.position) < aiScript.chaseSettings.chaseMinDistance)
            {
                Debug.Log("Calling Catching Target");

                CatchTarget();

                return;
            }

        return;
        
    }

    private void LosingTarget()
    {
        Debug.Log("Losing Target");

        losingTimer -= Time.deltaTime;

        if(losingTimer <= 0)
        {
            Debug.Log("Lost Target");

            aiScript.SwitchStates(StateMachineEnum.Search);

        }

        return;

    }

    private void CatchTarget()
    {
        Debug.Log("Can Catch Target");

        catchTimer -= Time.deltaTime;

        if(catchTimer <= 0)
        {
            Debug.Log("Attempting to Catch Target");
            CaughtTarget();
            
        }

        return;
    }

    public void CaughtTarget()
    {
        switch (aiScript.chaseSettings.chaseSpeciality) {


            case ChaseSpeciality.Push :
                {
                    Debug.Log("Pushing target");
                    break;
                }
            case ChaseSpeciality.Grab :
                {
                    Debug.Log("Grabbing Target");
                    break;
                }
        }

        Debug.Log("Caught Target");

    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Chase State");

        aiScript.agent.speed -= aiScript.chaseSettings.chaseSpeedIncrease;

        chasingTarget = null;

        aiScript.searchFunctionSettings.playerObj = null;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        aNewState.OnEnterState();

        OnExitState();

        return;
    }

}
