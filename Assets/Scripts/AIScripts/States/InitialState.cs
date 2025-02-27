using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialState : BaseStateClass
{
    public InitialState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("In Initial State");
        return;
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Initial State");
        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        

        aNewState.OnEnterState();

        OnExitState();

        return;

    }

    public override void CurrStateFunctionality()
    {
        aiScript.SwitchStates(aiScript.currActiveState, aiScript.idle);
        return;
    }

}
