using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Rendering;
using UnityEngine;

public class ChaseState : BaseStateClass
{
    float timer;

    float catchTimer;

    public ChaseState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }


    public TargetScript chasingTarget;
 
    public override void OnEnterState()
    {
        Debug.Log("Entering Chase State");
        chasingTarget = aiScript.RetrieveCurrTarget();

        aiScript.Speed += aiScript.chaseSpeedVal;

        timer = aiScript.losingTargetVal;

        catchTimer = aiScript.catchTimerVal;

        aiScript.agent.destination = chasingTarget.transform.position;

        return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("Chase Functionality");
        aiScript.agent.destination = chasingTarget.transform.position;
        
        if(Vector3.Distance(aiScript.gameObject.transform.position, chasingTarget.transform.position) > aiScript.chaseMaxDistance)
        {
            Debug.Log("Calling Losing Target");

            LosingTarget();
        }


        if(Vector3.Distance(aiScript.gameObject.transform.position, chasingTarget.transform.position) < aiScript.chaseMaxDistance)
        {
            Debug.Log("Calling Catching Target");

            CatchTarget();
        }
        
    }

    private void LosingTarget()
    {
        Debug.Log("Losing Target");

        timer -= aiScript.losingTargetVal;

        if(timer <= 0)
        {
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.search);
        }

        return;

    }

    private void CatchTarget()
    {
        Debug.Log("Catching Target");

        catchTimer -= Time.deltaTime;

        if(catchTimer <= 0)
        {
            CaughtTarget();
        }
    }

    public void CaughtTarget()
    {
        Debug.Log("Caught Target");

        
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Chase State");

        aiScript.Speed -= aiScript.chaseSpeedVal;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        aNewState.OnEnterState();

        OnExitState();

        return;
    }

}
