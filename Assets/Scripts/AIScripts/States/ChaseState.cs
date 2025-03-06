using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Rendering;
using UnityEngine;

public class ChaseState : BaseStateClass
{
    float losingTimer;

    float catchTimer;

    

    public ChaseState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }


    public TargetScript chasingTarget;
 
    public override void OnEnterState()
    {
        Debug.Log("Entering Chase State");
        chasingTarget = aiScript.RetrieveCurrTarget();

        aiScript.Speed += aiScript.chaseSettings.chaseSpeedVal;

        losingTimer = aiScript.chaseSettings.losingTargetVal;

        catchTimer = aiScript.chaseSettings.catchTimerVal;

        aiScript.agent.isStopped = false;

       
        return;
    }

    public override void CurrStateFunctionality()
    {
        //Debug.Log("Chase Functionality");
        
        
       
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

        losingTimer -= aiScript.chaseSettings.losingTargetVal;

        if(losingTimer <= 0)
        {
            Debug.Log("Lost Target");
            

            aiScript.interactSettings.CurrTarget = null;

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

     

        aiScript.Speed -= aiScript.chaseSettings.chaseSpeedVal;

        chasingTarget = null;

        aiScript.interactSettings.playerObj = null;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        //.Log("Changing From Chase State");

        aNewState.OnEnterState();

        OnExitState();

        return;
    }

}
