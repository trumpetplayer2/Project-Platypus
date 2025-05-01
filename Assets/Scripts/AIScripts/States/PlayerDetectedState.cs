using System.Collections;
using System.Collections.Generic;
using tp2;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDetectedState : BaseStateClass
{

    string aIResponse;

    string triggerResponse;

    ItemScript heldItem;

    bool itemTrigger;

    public PlayerDetectedState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    /// <summary>
    /// Gets reference to Regular and Trigger Responses
    /// </summary>
    public override void OnEnterState()
    {
        

        aIResponse = System.Enum.GetName(typeof(AIResponse), aiScript.playerDetectedSettings.setAIResponse);

        triggerResponse = System.Enum.GetName(typeof(TriggeredResponse), aiScript.playerDetectedSettings.setAITriggerResponse);

        if (aiScript.playerDetectedSettings.itemCheck)
        {
            heldItem = PlayerAbilityManager.instance.grab.heldObject;

            if(heldItem != null)
            {
                if(heldItem.type.ToString() == aiScript.playerDetectedSettings.heldObjTypeTrigger)
                {
                    itemTrigger = true;
                }
            }
        }

        return;
        
    }

    /// <summary>
    /// Will switch to the appropriate state depending on the set response
    /// </summary>
    public override void CurrStateFunctionality()
    {
        if (aiScript.playerDetectedSettings.TriggerDetected || itemTrigger)
        {
            
            TriggerBehavior();
        }
        else if(!aiScript.playerDetectedSettings.TriggerDetected || aiScript.playerDetectedSettings.setAITriggerResponse == TriggeredResponse.None)
        {
           

            switch (aIResponse)
            {
                case "Chase":
                    {
                        aiScript.aIAnimator.SetBool("Run", true);
                        aiScript.SwitchStates(StateMachineEnum.Chase);
                        break;
                    }
                case "Observe":
                    {
                        aiScript.SwitchStates(StateMachineEnum.Observe); break;
                    }

            }
        }

        return;
    }

    public override void OnExitState()
    {
        
        heldItem = null;

        itemTrigger = false;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        //Debug.Log("Switching States from Detected Player");

        aNewState.OnEnterState();

        OnExitState();

        return;
    }

    /// <summary>
    /// Will acquire the player object and switch to the required state based on the trigger behavior
    /// </summary>
    public void TriggerBehavior()
    {
       
        aiScript.searchFunctionSettings.playerObj = PlayerMovement.instance.GetComponent<TargetScript>();

        switch (triggerResponse)
        {
            case "None":
                {
                    return;
                }
            case "Chase":
                {
                    aiScript.SwitchStates(StateMachineEnum.Chase);
                    break;
                }
        }
    }
}
