using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseStateClass
{
    public InteractState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    GameObject currentTarget;

    public override void OnEnterState()
    {

        Debug.Log("In interact State");
        currentTarget = aiScript.RetrieveCurrTarget();
        aiScript.agent.isStopped = false;
        aiScript.agent.destination = currentTarget.transform.position;
        
        return;
    }

    public override void OnExitState()
    {

        aiScript.TargetsBacklog.Add(currentTarget);

        aiScript.CurrTarget = null;

        

        Debug.Log("Exiting interact State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
            Debug.Log("Changing from Patrol or Idle");

       

        OnExitState();

            return;
        
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("interact functionality");

        if (Vector3.Distance(currentTarget.transform.position, aiScript.gameObject.transform.position) < aiScript.TargetInteractDistance)
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

 

}
