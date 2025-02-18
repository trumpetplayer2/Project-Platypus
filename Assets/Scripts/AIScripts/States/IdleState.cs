using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BaseStateClass
{
    public override bool IsActiveState
    {
        get { return isActiveState; }

        set { isActiveState = value;

            if (!IsActiveState)
                DeactivateState();
            
            else
                 ActivateState();
        }

    }

    public override void OnEnterState()
    {

        Debug.Log("In idle State");
        
        StartCoroutine(aiScript.ChangeToPatrol(aiScript.idleTimeUntil));
        
        return;
    }

    public override void OnExitState()
    {
        StopCoroutine(aiScript.ChangeToPatrol(aiScript.idleTimeUntil));

        IsActiveState = false;
        Debug.Log("Exiting idle State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
            Debug.Log("Changing from Idle");

        aNewState.IsActiveState = true;


        OnExitState();

            return;

    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("idle functionality");

      
        aiScript.SearchForTargets();

        return;
    }

   

}
