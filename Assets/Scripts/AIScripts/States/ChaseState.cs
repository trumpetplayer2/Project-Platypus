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

    public ChaseState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Chase State");
        chasingTarget = aiScript.searchFunctionSettings.playerObj;

        aiScript.agent.speed += aiScript.chaseSettings.chaseSpeedIncrease;

        losingTimer = aiScript.chaseSettings.losingTargetTime;

        catchTimer = aiScript.chaseSettings.catchTargetTime;

       

        

        aiScript.agent.isStopped = false;

        return;
    }

    public override void CurrStateFunctionality()
    {
      

        aiScript.agent.destination = chasingTarget.transform.position;

            if (Vector3.Distance(aiScript.searchFunctionSettings.Eyes.gameObject.transform.position, chasingTarget.transform.position) > aiScript.chaseSettings.chaseMaxDistance)
            {
                Debug.Log("Calling Losing Target");

                LosingTarget();

                return;
            }


            if (Vector3.Distance(aiScript.searchFunctionSettings.Eyes.gameObject.transform.position, chasingTarget.transform.position) < aiScript.chaseSettings.chaseMinDistance)
            {
                
                Debug.Log("Calling Catching Target");

                CatchTarget();

                return;
            }

       
            if(catchCoolingDown)
            {
                Debug.Log("catch Cooling Down");
                
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

    private void LosingTarget()
    {
        Debug.Log("Losing Target");

        losingTimer -= Time.deltaTime;

        if(losingTimer <= 0)
        {
            Debug.Log("Lost Target");

            aiScript.SwitchStates(StateMachineEnum.Search);

        }

        return;

    }

    private void CatchTarget()
    {
        
        Debug.Log("Can Catch Target");

        catchTimer -= Time.deltaTime;

        if(catchTimer <= 0)
        {
            Debug.Log("Attempting to Catch Target");
           
            GrabFunction();

            
            return;
            
        }

        return;
    }

   

    private void GrabFunction()
    {
        if(catchCoolingDown)
        {
            return;
        }

        Debug.Log("Grab Function");

        if(chasingTarget.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            player.held = true;
            player.transform.position = aiScript.chaseSettings.playerGrabbedPosition.transform.position;
            player.transform.parent = aiScript.transform;


            Debug.Log("Taking player to location");

            aiScript.agent.destination = aiScript.chaseSettings.grabbedPlayerLocation.checkpointPosition;

            if (Vector3.Distance(aiScript.transform.position, aiScript.chaseSettings.grabbedPlayerLocation.checkpointPosition) < 6f)
            {
                aiScript.agent.isStopped = true;

                Debug.Log("Am I here?");
                player.held = false;

                catchTimer = aiScript.chaseSettings.catchTargetTime;

                player.transform.parent = null;

                catchCoolingDown = true;

            }
        }

    }

   
    public override void OnExitState()
    {
        Debug.Log("Exiting Chase State");

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
