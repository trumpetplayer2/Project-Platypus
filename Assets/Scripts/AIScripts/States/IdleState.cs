using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BaseStateClass
{
    public IdleState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }
    
    float timer;
    public override void OnEnterState()
    {
        
        Debug.Log("In idle State");

        timer = aiScript.idleTimeUntil;
        
        return;
    }

    public override void OnExitState()
    {

        Debug.Log("Exiting idle State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
            Debug.Log("Changing from Idle");

        aNewState.OnEnterState();

        OnExitState();

            return;

    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("idle functionality");

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            Debug.Log("Switching to Patrol");
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.patrol);
        }

        if (aiScript.SearchForTargets())
        {
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.interact);
        }

        return;
    }

}
