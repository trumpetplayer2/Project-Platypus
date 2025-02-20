using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ChaseState : BaseStateClass
{
    public ChaseState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }


    public TargetScript chasingTarget;
 
    public override void OnEnterState()
    {
        Debug.Log("Entering Chase State");
        chasingTarget = aiScript.RetrieveCurrTarget();
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("Chase Functionality");
        aiScript.agent.destination = chasingTarget.transform.position;
         
        
    }

    private void LosingTarget()
    {
        Debug.Log("Lost Target");
       
    }

    private void CatchTarget()
    {
        Debug.Log("Caught Target");
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Chase State");
        
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        

        OnExitState();
    }

}
