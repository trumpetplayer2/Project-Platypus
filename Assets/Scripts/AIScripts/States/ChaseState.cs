using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Rendering;
using UnityEngine;

public class ChaseState : BaseStateClass
{
    float losingTimer;

    float catchTimer;

    bool seesPlayer;

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

        losingTimer = aiScript.losingTargetVal;

        catchTimer = aiScript.catchTimerVal;

        aiScript.agent.isStopped = false;

       
        return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("Chase Functionality");
        
        
       
            aiScript.agent.destination = chasingTarget.transform.position;

            if (Vector3.Distance(aiScript.Eyes.gameObject.transform.position, chasingTarget.transform.position) > aiScript.chaseMaxDistance)
            {
                Debug.Log("Calling Losing Target");

                LosingTarget();
            }


            if (Vector3.Distance(aiScript.Eyes.gameObject.transform.position, chasingTarget.transform.position) < aiScript.chaseMinDistance)
            {
                Debug.Log("Calling Catching Target");

                CatchTarget();
            }
        
        

        return;
        
    }

    private void LosingTarget()
    {
        Debug.Log("Losing Target");

        losingTimer -= aiScript.losingTargetVal;

        if(losingTimer <= 0)
        {
            Debug.Log("Lost Target");
            
            aiScript.playerFound = false;

            aiScript.CurrTarget = null;

            aiScript.SwitchStates(aiScript.currActiveState, aiScript.search);
           
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
        Debug.Log("Caught Target");

        
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Chase State");

        seesPlayer = false;

        aiScript.Speed -= aiScript.chaseSpeedVal;

        chasingTarget = null;

        aiScript.playerObj = null;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Changing From Chase State");

        aNewState.OnEnterState();

        OnExitState();

        return;
    }

}
