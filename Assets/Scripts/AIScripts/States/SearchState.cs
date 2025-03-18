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


    Vector3 detectedLocation;

    float noiseTimer;

    public SearchState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }
    public override void OnEnterState()
    {
        Debug.Log("In Search State");

        if(aiScript.searchStateSettings.alreadyHeardSomething)
        {
           aiScript.SwitchStates(aiScript.patrol);
        }

        if (aiScript.searchStateSettings.heardSomething)
        {
          

            detectedLocation = aiScript.searchStateSettings.noiseLocation;

            aiScript.agent.isStopped = false;

            noiseTimer = aiScript.searchStateSettings.heardNoiseTime;
        }
        else
        {
            aiScript.agent.isStopped = true;

            aiScript.search.searchingForPlayer = true;

            timer = aiScript.searchStateSettings.searchStateTime;

            aISearchMethod = System.Enum.GetName(typeof(SearchMethod), aiScript.searchStateSettings.searchMethod);
        }

        return;
    }

    public override void CurrStateFunctionality()
    {
        if (aiScript.SearchForTargets() == DetectedType.Player)
        {
            Debug.Log("Player Found Again");
            aiScript.SwitchStates(aiScript.playerDetected);
        }

        if (aiScript.searchStateSettings.heardSomething)
        {
            SearchNoiseLocation();
        }

        if (searchingForPlayer)
        {
            Debug.Log("In Searching for Player statement");
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
        Debug.Log("Searching Location of Noise");

        aiScript.agent.destination = detectedLocation;

        if(Vector3.Distance(aiScript.transform.position, detectedLocation) <= 0f)
        {
            aiScript.agent.isStopped = true;

            noiseTimer -= Time.deltaTime;

            if (noiseTimer <= 0)
            {
                aiScript.searchStateSettings.alreadyHeardSomething = true;

                aiScript.searchStateSettings.heardSomething = false;

                

                Debug.Log("Should Change to This");
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
