using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetectedState : BaseStateClass
{
    TargetScript playerTarget;

    string aIResponse;

    public PlayerDetectedState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Player Detected State");

        playerTarget = aiScript.searchFunctionSettings.playerObj == null ? null : aiScript.searchFunctionSettings.playerObj;

        aIResponse = System.Enum.GetName(typeof(StateMachineInfo.PlayerDetectedSettings.AIResponse), aiScript.playerDetectedSettings.SetPlayerResponse);

        return;
        
    }

    public override void CurrStateFunctionality()
    {
        switch (aIResponse)
        {
            case "Chase":
                {
                    aiScript.SwitchStates(aiScript.chase);
                    break;
                }
            case "Observe":
                {
                    aiScript.SwitchStates(aiScript.observe); break;
                }

        }

        return;
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Player Detected State");

        playerTarget = null;

        
        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        //Debug.Log("Switching States from Detected Player");

        aNewState.OnEnterState();

        OnExitState();

        return;
    }
}
