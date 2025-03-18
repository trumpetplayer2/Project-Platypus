using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetectedState : BaseStateClass
{
    TargetScript playerTarget;

    string aIResponse;

    string triggerResponse;

    public PlayerDetectedState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Player Detected State");

        playerTarget = aiScript.searchFunctionSettings.playerObj == null ? null : aiScript.searchFunctionSettings.playerObj;

        aIResponse = System.Enum.GetName(typeof(AIResponse), aiScript.playerDetectedSettings.setAIResponse);

        triggerResponse = System.Enum.GetName(typeof(TriggeredResponse), aiScript.playerDetectedSettings.setAITriggerResponse);

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

    public void TriggerBehavior()
    {
        if (aiScript.searchFunctionSettings.playerObj == null)
            return;


        switch (triggerResponse)
        {
            case "None":
                {
                    break;
                }

            case "Chase":
                {
                    aiScript.SwitchStates(aiScript.chase);
                    break;
                }
        }
    }
}
