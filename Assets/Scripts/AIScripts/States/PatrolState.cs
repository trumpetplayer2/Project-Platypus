using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseStateClass
{
    public PatrolState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    int randomPatrolDestination;

    public override void OnEnterState()
    {
        Debug.Log("In Patrol State");

        randomPatrolDestination = Random.Range(0, aiScript.patrolSettings.PatrolDestinations.Length - 1);

        aiScript.agent.isStopped = false;

        return;
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Patrol State");
        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    { 
           aNewState.OnEnterState();

            OnExitState();

            return;
        
    }

    public override void CurrStateFunctionality()
    {
        aiScript.patrolSettings.CurrPatrolDestination = aiScript.patrolSettings.PatrolDestinations[randomPatrolDestination];

        aiScript.agent.destination = aiScript.patrolSettings.PatrolDestinations[randomPatrolDestination].position;

        if ((Vector3.Distance(aiScript.gameObject.transform.position, aiScript.patrolSettings.CurrPatrolDestination.position)) < aiScript.patrolSettings.patrolDistanceToDestination)
        {
            randomPatrolDestination = Random.Range(0, aiScript.patrolSettings.PatrolDestinations.Length - 1);
        }

        if (aiScript.SearchForTargets() == DetectedType.Object)
        {
            Debug.Log("Switching to Interact");
            aiScript.SwitchStates(aiScript.interact);
            return;
        }
        else if(aiScript.SearchForTargets() == DetectedType.Player)
        {
            Debug.Log("Switching to Player Detected");
            aiScript.SwitchStates(aiScript.playerDetected);

            return;
        }

        return;
    }

  
}
