using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using tp2;
using UnityEditor.Rendering;
using UnityEngine;

public class ChaseState : BaseStateClass
{
    public TargetScript chasingTarget;

    float losingTimer;

    float catchTimer;

    bool catchCoolingDown = false;

    float timer = 0;

    bool callGrabAnim;



    public ChaseState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    
    public override void OnEnterState()
    {
        //Debug.Log("Entering Chase State");
        chasingTarget = aiScript.searchFunctionSettings.playerObj;

        aiScript.agent.speed += aiScript.chaseSettings.chaseSpeedIncrease;

        losingTimer = aiScript.chaseSettings.losingTargetTime;

        catchTimer = aiScript.chaseSettings.catchTargetTime;

        aiScript.agent.isStopped = false;

        aiScript.aIAnimator.SetBool("Run", true);

        callGrabAnim = false;

        return;
    }

    /// <summary>
    /// will begin to chase the player, and check if the player exceeds or is below a specific distance
    /// 
    /// also operates catch cooldown for the AI, so that it won't grab the player immediately upon letting them go
    /// </summary>
    public override void CurrStateFunctionality()
    {
        if(PlayerMovement.instance.held == true && !aiScript.chaseSettings.iCaughtPlayer)
        {
            aiScript.playerDetectedSettings.playerDetectedCooldown = true;
        }
  
        if(aiScript.playerDetectedSettings.playerDetectedCooldown)
        {
            LosingTarget();
        }

        aiScript.agent.destination = chasingTarget.transform.position;

            if (Vector3.Distance(aiScript.searchFunctionSettings.Eyes.gameObject.transform.position, chasingTarget.transform.position) > aiScript.chaseSettings.chaseMaxDistance)
            {
                

                LosingTarget();

                return;
            }

            if (Vector3.Distance(aiScript.searchFunctionSettings.Eyes.gameObject.transform.position, chasingTarget.transform.position) <= aiScript.chaseSettings.chaseMinDistance)
            {

             //Debug.Log("Am I close to the player");
                CatchTarget();

                return;
            }

            if(catchCoolingDown)
            {
               
                
                aiScript.agent.isStopped = false;

                timer += Time.deltaTime;

                if(timer >= aiScript.chaseSettings.catchCooldown)
                {
                    catchCoolingDown = false;
                    timer = 0;
                }
            }

        return;
        
    }

    /// <summary>
    /// is called if the player exceeds a specific range, and will switch to search state when the timer reaches 0 or below
    /// </summary>
    private void LosingTarget()
    {
       

        losingTimer -= Time.deltaTime;

        if(losingTimer <= 0)
        {
           

            aiScript.SwitchStates(StateMachineEnum.Search);

        }

        return;

    }

    /// <summary>
    /// is called if the player is below a specific range from the AI, and will call the GrabFunction when the timer reaches 0 or below
    /// </summary>
    private void CatchTarget()
    {
       

        catchTimer -= Time.deltaTime;

        if(catchTimer <= 0)
        {
            if (!callGrabAnim)
            {
                //Debug.Log("Is this Running");
               
                aiScript.aIAnimator.SetTrigger("PickUP");
                aiScript.agent.destination = aiScript.transform.position;
                aiScript.agent.isStopped = true;
                callGrabAnim = true;
            }

            //Debug.Log("Calling Grabbing Function");
            GrabFunction();

            return;
            
        }

        return;
    }


    /// <summary>
    /// will grab the player and take them to the checkpoint location
    /// </summary>
    private void GrabFunction()
    {
        if (aiScript.aIAnimator.GetCurrentAnimatorStateInfo(0).IsName("Base.PickUP") || catchCoolingDown)
        {
            return;
        }

        //Debug.Log("Start of Grab Function");

        //aiScript.aIAnimator.SetBool("Run", false);

        PlayerMovement player = PlayerMovement.instance;
        PlayerAbilityManager playerInstance = PlayerAbilityManager.instance;

        aiScript.agent.isStopped = false;



        aiScript.aIAnimator.SetBool("Walk", true);
        aiScript.aIAnimator.SetBool("Run", false);
        if (!GameManager.instance.isPaused && !PauseScript.instance.returningtoCheckpoint)
        {
            player.held = true;

            aiScript.chaseSettings.iCaughtPlayer = true;

            playerInstance.Release();
            player.transform.position = aiScript.chaseSettings.playerGrabbedPosition.transform.position;
        }

        //Debug.Log("Here");

        aiScript.agent.destination = aiScript.chaseSettings.grabbedPlayerLocation.checkpointPosition;



        if (Vector3.Distance(aiScript.transform.position, aiScript.chaseSettings.grabbedPlayerLocation.checkpointPosition) < aiScript.chaseSettings.distanceToCheckpoint)
        {

            aiScript.agent.isStopped = true;


            player.held = false;

            catchTimer = aiScript.chaseSettings.catchTargetTime;

            player.transform.parent = null;

            catchCoolingDown = true;

            aiScript.playerDetectedSettings.playerDetectedCooldown = true;

            aiScript.SwitchStates(StateMachineEnum.Patrol);

            //aiScript.aIAnimator.SetBool("Walk", true);


        }

    }

   
    public override void OnExitState()
    {
       

        aiScript.agent.speed -= aiScript.chaseSettings.chaseSpeedIncrease;

        chasingTarget = null;

        aiScript.searchFunctionSettings.playerObj = null;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        aNewState.OnEnterState();

        OnExitState();

        return;
    }

}
