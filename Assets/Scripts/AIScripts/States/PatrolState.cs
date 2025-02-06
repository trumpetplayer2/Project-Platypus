using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : BaseStateClass
{
    int randomPatrolDestination;
    //public PatrolState(AIBase aAIBase) : base(aAIBase)
    //{

    //}

    public override void StateSetup(AIBase aAIscript)
    {

        Debug.Log("Setting Up Patrol State");
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("In Patrol State");

        //IsActiveState = true;
        randomPatrolDestination = Random.Range(0, aiScript.PatrolDestinations.Length - 1);

        //aiScript.currActiveState = aiScript.patrol;

        return;
    }

    public override void OnExitState()
    {

        //IsActiveState = false;
        Debug.Log("Exiting Patrol State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState, ref BaseStateClass aCurrState)
    {

        Debug.Log("Changing from Patrol State");
        aCurrState.OnExitState();


        aNewState.OnEnterState();

        return;
    }

    //public override void OnEveryFrame()
    //{
    //    Debug.Log("Checking for player");
    //}

    public override void CurrStateFunctionality()
    {

        Debug.Log("Patrolling to target");

      


        aiScript.CurrPatrolDestination = aiScript.PatrolDestinations[randomPatrolDestination];


        aiScript.agent.destination = aiScript.PatrolDestinations[randomPatrolDestination].position;



        if ((Vector3.Distance(aiScript.gameObject.transform.position, aiScript.CurrPatrolDestination.position)) < 2)
        {

            randomPatrolDestination = Random.Range(0, aiScript.PatrolDestinations.Length - 1);


        }
    }

    /// <summary>
    /// 
    /// </summary>
   




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //zOnEveryFrame();
    }
}
