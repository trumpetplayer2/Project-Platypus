using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseStateClass
{

    public override bool IsActiveState
    {
        get { return isActiveState; }

        set
        {
            isActiveState = value;

            if (!IsActiveState)
                DeactivateState();

        }

    }

    GameObject currentTarget;

    public void SetTarget(GameObject NewTarget)
    {
        
        
            Debug.Log("New Target for Interact State is set");
            currentTarget = NewTarget;
        

        return;
    }

    public override void OnEnterState()
    {

        Debug.Log("In interact State");
        aiScript.agent.isStopped = false;
        aiScript.agent.destination = currentTarget.transform.position;
        
        return;
    }

    public override void OnExitState()
    {

        aiScript.TargetsBacklog.Add(currentTarget);

        aiScript.CurrTarget = null;

        IsActiveState = false;

        Debug.Log("Exiting interact State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState, ref BaseStateClass aCurrState)
    {
        
        if (!aiScript.CheckForStateCooldown())
        {
            Debug.Log("Changing from Patrol or Idle");
            aCurrState.OnExitState();

            aNewState.OnEnterState();

            return;
        }
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("interact functionality");

        if (Vector3.Distance(currentTarget.transform.position, aiScript.gameObject.transform.position) < 2)
        {
            Debug.Log("AI is stopped in front of target");

            BeginInteract(currentTarget);
        }

        return;
        
    }

    private void BeginInteract(GameObject aCurrentTarget)
    {
        Debug.Log("Beginning interacting");

        string TargetInstructions = aCurrentTarget.tag.ToString();

        switch (TargetInstructions)
        {
            case "Player": 
                    {
                    aiScript.PlayerInteractFunction();
                    break;
                    }
            case "Target":
                {
                    aiScript.BaseTargetInteractFunction();
                    break;
                }
            case "LowestPriority":
                {
                    aiScript.LowPriInteractFunction();
                    break;
                }
        }

    }

    public override void DeactivateState()
    {
        
    }

}
