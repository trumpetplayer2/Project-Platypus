using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialState : BaseStateClass
{
    public InitialState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {

        return;
    }

    public override void OnExitState()
    {

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Changing from Initial State");

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
