using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ChaseState : BaseStateClass
{

    public GameObject chasingTarget;
    public override bool IsActiveState
    {
        get { return isActiveState; }

        set
        {
            isActiveState = value;

            if (!IsActiveState)
                DeactivateState();

        }

    }
    public override void OnEnterState()
    {
        Debug.Log("Entering Chase State");
        chasingTarget = aiScript.RetrieveCurrTarget();
    }

    public override void CurrStateFunctionality()
    {
        aiScript.agent.destination = chasingTarget.transform.position;

        if(Vector3.Distance(this.gameObject.transform.position, chasingTarget.transform.position) < aiScript.TargetInteractDistance)
        {
            CatchTarget();
        }

        if(Vector3.Distance(this.gameObject.transform.position, chasingTarget.transform.position) > aiScript.ChaseRange)
        {
            
            LosingTarget();
        }
    }

    private void LosingTarget()
    {
        aiScript.LostTarget();
    }

    private void CatchTarget()
    {
        
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Chase State");
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        aNewState.IsActiveState = true;

        OnExitState();
    }

}
