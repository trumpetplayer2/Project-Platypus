using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : BaseStateClass
{
    TargetScript playerTarget;

    public PlayerDetectedState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Player Detected State");
        playerTarget = aiScript.RetrieveCurrTarget();


    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("Player Functionality");
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Player Detected State");
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Switching States from Detected Player");


        aNewState.OnEnterState();

        OnExitState();

        return;
    }
}
