using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseStateClass
{

    public Transform prevDestination;
    public PatrolState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    int randomPatrolDestination;

    public override void OnEnterState()
    {
        aiScript.aIAnimator.SetBool("Walk", true);

        randomPatrolDestination = Random.Range(0, aiScript.patrolSettings.PatrolDestinations.Length - 1);

        aiScript.agent.isStopped = false;

       

        return;
    }

    public override void OnExitState()
    {
        aiScript.aIAnimator.SetBool("Walk", false);
        
        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    { 
           aNewState.OnEnterState();

            OnExitState();

            return;
        
    }

    /// <summary>
    /// Travels to a randomly selected Transform location from an array of transforms, travels to the location, and then select a new location
    /// </summary>
    public override void CurrStateFunctionality()
    {
        aiScript.patrolSettings.CurrPatrolDestination = aiScript.patrolSettings.PatrolDestinations[randomPatrolDestination];

        aiScript.agent.destination = aiScript.patrolSettings.PatrolDestinations[randomPatrolDestination].position;

        if ((Vector3.Distance(aiScript.gameObject.transform.position, aiScript.patrolSettings.CurrPatrolDestination.position)) < aiScript.patrolSettings.patrolDistanceToDestination)
        {
           // Debug.Log("Switched Destination");
            randomPatrolDestination = Random.Range(0, aiScript.patrolSettings.PatrolDestinations.Length);
        }

        if (aiScript.SearchForTargets() == DetectedType.Object)
        {
            
            aiScript.SwitchStates(StateMachineEnum.Interact);
            return;
        }
        else if(aiScript.SearchForTargets() == DetectedType.Player)
        {
           
            aiScript.SwitchStates(StateMachineEnum.PlayerDetected);

            return;
        }

        return;
    }

  
}
