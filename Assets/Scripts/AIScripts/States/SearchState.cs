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
           aiScript.SwitchStates(StateMachineEnum.Patrol);
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

           
        }

        return;
    }

    //Searches for the player through a selected behavior
    public override void CurrStateFunctionality()
    {
        if (aiScript.SearchForTargets() == DetectedType.Player)
        {
            Debug.Log("Player Found Again");
            aiScript.SwitchStates(StateMachineEnum.PlayerDetected);
        }

        if (aiScript.searchStateSettings.heardSomething)
        {
            Debug.Log("Calling Search Noise Location Function");
            SearchNoiseLocation();
        }
        else
        {
            SearchInPlaceFunction();
        }

        return;
    }

    /// <summary>
    /// Searches in place for an inputted amount of time before changing states
    /// </summary>
    public void SearchInPlaceFunction()
    {
        Debug.Log("Searching in Place");

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Debug.Log("Resuming Patrol State from Search State");

            aiScript.SwitchStates(StateMachineEnum.Patrol);
            return;
        }

        return;
    }
   

    /// <summary>
    /// searching the location where a noise was detected, standing in place for an inputted amount of time, before returning to the patrol State
    /// </summary>
    public void SearchNoiseLocation()
    {
        Debug.Log("Searching Location of Noise");

        aiScript.agent.destination = detectedLocation;

        if(Vector3.Distance(aiScript.transform.position, detectedLocation) <= 0.5)
        {
            aiScript.agent.isStopped = true;

            noiseTimer -= Time.deltaTime;

            if (noiseTimer <= 0)
            {
                Debug.Log("Am I here");

                aiScript.searchStateSettings.alreadyHeardSomething = true;

                aiScript.searchStateSettings.heardSomething = false;

                Debug.Log("Should Change to This");
                aiScript.SwitchStates(StateMachineEnum.Patrol);
            }
        }

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
