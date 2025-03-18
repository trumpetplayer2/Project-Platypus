using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SearchState : BaseStateClass
{
    float timer;

    string aISearchMethod;

    public bool searchingForPlayer;

    bool heardSomething;

    Vector3 detectedLocation;

    float noiseTimer;

    public SearchState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }
    public override void OnEnterState()
    {
        if(aiScript.searchStateSettings.heardSomething)
        {
            heardSomething = true;

            detectedLocation = aiScript.searchStateSettings.noiseLocation;

            aiScript.agent.isStopped = false;

            noiseTimer = aiScript.searchStateSettings.heardNoiseTime;
        }

        Debug.Log("In Search State");

        aiScript.agent.isStopped = true;

        aiScript.search.searchingForPlayer = true;

        timer = aiScript.searchStateSettings.searchStateTime;

        aISearchMethod = System.Enum.GetName(typeof(SearchMethod), aiScript.searchStateSettings.searchMethod);

        return;
    }

    public override void CurrStateFunctionality()
    {
        if (aiScript.SearchForTargets() == DetectedType.Player)
        {
            Debug.Log("Player Found Again");
            aiScript.SwitchStates(aiScript.playerDetected);
        }

        if (heardSomething)
        {
            SearchNoiseLocation();
        }

        if (searchingForPlayer)
        {
            switch (aISearchMethod)
            {
                case "SearchInPlace":
                    {
                        SearchInPlaceFunction();
                        break;
                    }
             
            }
            
        }

        return;
    }

 
    public void SearchInPlaceFunction()
    {
        Debug.Log("Searching in Place");

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Debug.Log("Resuming Patrol State from Search State");

            aiScript.SwitchStates(aiScript.patrol);
            return;
        }

        return;
    }
   
    public void SearchNoiseLocation()
    {
        Debug.Log("Searching Random Point in Range");

        aiScript.agent.destination = detectedLocation;

        if(Vector3.Distance(aiScript.transform.position, detectedLocation) <= 1)
        {
            aiScript.agent.isStopped = true;

            noiseTimer -= Time.deltaTime;

            if (noiseTimer <= 0)
            {
                aiScript.SwitchStates(aiScript.patrol);
            }
        }

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
