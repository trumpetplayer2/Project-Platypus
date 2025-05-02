using System.Collections;
using System.Collections.Generic;
using tp2;
using Unity.VisualScripting;
using UnityEngine;



public class ObserveState : BaseStateClass
{
    public TargetScript observedTarget;

    Transform currPosition;

   

    float timer = 0f;

    public float timerMax;
    
    public Vector3 start;

    public Vector3 goal;
    private ItemScript heldItem;

    public bool itemTrigger;

    bool itemDetected;

    public ObserveState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        //aiScript.aIAnimator.SetBool("Walk", false);

        aiScript.aIAnimator.SetBool("Interact", true);

        observedTarget = aiScript.searchFunctionSettings.playerObj;

        currPosition = aiScript.gameObject.transform;

        aiScript.agent.isStopped = true;

        timerMax = aiScript.observeSettings.observeTimerMax;


       

       

        return;
    }

    /// <summary>
    /// Will look at the player if they are within range, and will switch to search state when the player moves far enough.
    /// </summary>
    public override void CurrStateFunctionality()
    {

        if (aiScript.playerDetectedSettings.itemCheck)
        {
            heldItem = PlayerAbilityManager.instance.grab.heldObject;

            if (heldItem != null)
            {
                if (heldItem.type.ToString() == aiScript.playerDetectedSettings.heldObjTypeTrigger)
                {
                    itemTrigger = true;
                    itemDetected = true;

                }
            }
        }

        if (aiScript.playerDetectedSettings.TriggerDetected || itemTrigger)
        {

            aiScript.SwitchStates(StateMachineEnum.Chase);
        }

     if (!itemDetected)
     {

            if (goal == null || timer >= timerMax)
        {
            start = aiScript.transform.forward;

            goal = (observedTarget.transform.position - aiScript.transform.position).normalized;

            timer = 0f;


        }

        timer += Time.deltaTime;

        aiScript.transform.forward = Vector3.Lerp(start, goal, timer / timerMax);

       

       
            if (Vector3.Distance(observedTarget.transform.position, currPosition.position) >= aiScript.observeSettings.maxObserveDistance)
            {

                aiScript.SwitchStates(StateMachineEnum.Search);
                return;
            }
     }

       

    }

    public override void OnExitState()
    {
       

        observedTarget = null;

        aiScript.searchFunctionSettings.playerObj = null;

        heldItem = null;

        itemTrigger = false;


        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        aNewState.OnEnterState();

        OnExitState();

        return;
    }
}
