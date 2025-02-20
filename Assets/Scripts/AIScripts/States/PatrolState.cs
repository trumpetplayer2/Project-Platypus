using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseStateClass
{
    public PatrolState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    int randomPatrolDestination;

    public override void OnEnterState()
    {
        Debug.Log("In Patrol State");

        randomPatrolDestination = Random.Range(0, aiScript.PatrolDestinations.Length - 1);

        aiScript.agent.isStopped = false;

        return;
    }

    public override void OnExitState()
    {
       // aiScript.agent.isStopped = true;
        
       
        Debug.Log("Exiting Patrol State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    { 
            Debug.Log("Changing from Patrol State");

           aNewState.OnEnterState();

            OnExitState();

            return;
        
    }

    public override void CurrStateFunctionality()
    {
       
      

        Debug.Log("Patrolling to destination");


        aiScript.CurrPatrolDestination = aiScript.PatrolDestinations[randomPatrolDestination];

        aiScript.agent.destination = aiScript.PatrolDestinations[randomPatrolDestination].position;

        if ((Vector3.Distance(aiScript.gameObject.transform.position, aiScript.CurrPatrolDestination.position)) < 2)
        {
            randomPatrolDestination = Random.Range(0, aiScript.PatrolDestinations.Length - 1);
        }

        if (aiScript.SearchForTargets())
        { 
            
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.interact);
        }

        
        return;
    }

  
}
