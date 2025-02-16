using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractState : BaseStateClass
{
    //public override bool IsActiveState
    //{

    //    get { return isActiveState; }


    //    set
    //    {
    //        isActiveState = value;

    //        if (!IsActiveState)
    //        {
    //            this.gameObject.SetActive(false);
    //        }
    //    }
    //}


    GameObject currentTarget;

    public override void StateSetup(AIBase aAIscript)
    {
        this.aiScript = aAIscript;

        //idleTimeUntil = 5;

        //Debug.Log("Setting Up Interact State");
    }

    public void SetTarget(GameObject NewTarget)
    {
        if(NewTarget == currentTarget)
        {
            return;
        }
        else
        {
            Debug.Log("New Target for Interact State is set");
            currentTarget = NewTarget;
        }
       

        return;
    }

    public override void OnEnterState()
    {

        Debug.Log("In interact State");
        aiScript.agent.isStopped = false;
        aiScript.agent.destination = currentTarget.transform.position;
        //IsActiveState = true;


        return;
    }

    public override void OnExitState()
    {
        

        //IsActiveState = false;

        Debug.Log("Exiting interact State");

    
        //IsActiveState = false;
        return;
    }

    public override void ChangeState(BaseStateClass aNewState, ref BaseStateClass aCurrState)
    {
        Debug.Log("Changing from Patrol or Idle");
        aCurrState.OnExitState();


        aNewState.OnEnterState();

        return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("interact functionality");

        //aiScript.agent.isStopped = true;

        //BeginInteract(currentTarget);

        //Debug.Log(Vector3.Distance(aiScript.gameObject.transform.position, currentTarget.transform.position));

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

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
