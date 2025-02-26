using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseStateClass
{
    float timer;

    string aISearchMethod;

    public bool searchingForPlayer;

    bool heardSomething;

    Vector3 detectedLocation;

    float angle;

    public SearchState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }
    public override void OnEnterState()
    {
        Debug.Log("In Search State");

       

        aiScript.search.searchingForPlayer = true;

        timer = aiScript.searchStateVal;

        aISearchMethod = System.Enum.GetName(typeof(AIBase.SearchMethod), aiScript.searchMethod);

        

        return;
    }

    public override void CurrStateFunctionality()
    {

        if (!aiScript.SearchForTargets() && aiScript.playerFound)
        {
            Debug.Log("Player Found Again");
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.playerDetected);
        }


        Debug.Log("Search Functionality");

        if (searchingForPlayer)
        {
            switch (aISearchMethod)
            {
                case "SearchInPlace":
                    {
                        SearchInPlaceFunction();
                        break;
                    }
                case "SearchInRandomPoint":
                    {
                        SearchRandomPointFunction();
                        break;
                    }
            }
        }

    }

 
    public void SearchInPlaceFunction()
    {
        Debug.Log("Searching in Place");

        aiScript.agent.isStopped = true;
        timer -= Time.deltaTime;

       

        if (timer <= 0)
        {
            Debug.Log("Resuming Patrol State from Search State");

            searchingForPlayer = false;
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.patrol);
        }

        return;
    }
   
    public void SearchRandomPointFunction()
    {
        Debug.Log("Searching Random Point in Range");

        return;
    }

    public void MoveToPointFunction()
    {
        Debug.Log("Investigating Point");

        return;
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Search State");

        aiScript.agent.isStopped = false;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Changing from Search State");

        aNewState.OnEnterState();

        OnExitState();

        return;
    }

    
}
