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
           
            aiScript.SwitchStates(StateMachineEnum.PlayerDetected);
        }

        if (aiScript.searchStateSettings.heardSomething)
        {
           
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
        if (aiScript.aIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base.Walk"))
        {
            aiScript.aIAnimator.SetBool("Walk", false);
        }
        else if (aiScript.aIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base.Run"))
        {
            aiScript.aIAnimator.SetBool("Run", false);
        }

        aiScript.aIAnimator.SetBool("Interact", true);

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
           

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
       

        aiScript.agent.destination = detectedLocation;

        if(Vector3.Distance(aiScript.transform.position, detectedLocation) <= 0.5)
        {
            aiScript.agent.isStopped = true;

            noiseTimer -= Time.deltaTime;

            if (noiseTimer <= 0)
            {
               

                aiScript.searchStateSettings.alreadyHeardSomething = true;

                aiScript.searchStateSettings.heardSomething = false;

                
                aiScript.SwitchStates(StateMachineEnum.Patrol);
            }
        }

        return;
    }

    public override void OnExitState()
    {
        

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
       

        aNewState.OnEnterState();

        OnExitState();

        return;
    }

    
}
