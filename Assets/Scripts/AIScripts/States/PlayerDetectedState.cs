using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetectedState : BaseStateClass
{
    TargetScript playerTarget;

    string aIResponse;

    public PlayerDetectedState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Player Detected State");

        playerTarget = aiScript.RetrieveCurrTarget();

        aIResponse = System.Enum.GetName(typeof(AIBase.AIResponse), aiScript.SetPlayerResponse);

        return;
        
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("Player Functionality");

       
        switch (aIResponse)
        {
            case "Chase":
                {
                    aiScript.SwitchStates(aiScript.currActiveState, aiScript.chase);
                    break;
                }

        }


    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Player Detected State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Switching States from Detected Player");


        aNewState.OnEnterState();

        OnExitState();

        return;
    }
}
