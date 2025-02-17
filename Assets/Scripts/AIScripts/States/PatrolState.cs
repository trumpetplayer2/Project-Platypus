using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseStateClass
{

    public override bool IsActiveState
    { 
        get { return isActiveState; }

        set
        {
            isActiveState = value;

            if(!isActiveState)
                DeactivateState();
        }
    }

    int randomPatrolDestination;

    public override void OnEnterState()
    {
        Debug.Log("In Patrol State");

        randomPatrolDestination = Random.Range(0, aiScript.PatrolDestinations.Length - 1);

        return;
    }

    public override void OnExitState()
    {
        aiScript.agent.isStopped = true;
        
        IsActiveState = false;
        Debug.Log("Exiting Patrol State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState, ref BaseStateClass aCurrState)
    { 
        
        if (!aiScript.CheckForStateCooldown())
        {
            Debug.Log("Changing from Patrol State");
            aCurrState.OnExitState();

            aNewState.OnEnterState();

            return;
        }
    }

    public override void CurrStateFunctionality()
    {
        if (!aiScript.isSearchCooldowm)
            aiScript.SearchForTargets();

        Debug.Log("Patrolling to target");

        aiScript.CurrPatrolDestination = aiScript.PatrolDestinations[randomPatrolDestination];

        aiScript.agent.destination = aiScript.PatrolDestinations[randomPatrolDestination].position;

        if ((Vector3.Distance(aiScript.gameObject.transform.position, aiScript.CurrPatrolDestination.position)) < 2)
        {
            randomPatrolDestination = Random.Range(0, aiScript.PatrolDestinations.Length - 1);
        }



        return;
    }

    public override void DeactivateState()
    {
        
    }
}
