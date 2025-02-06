using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : BaseStateClass
{
    //public override bool IsActiveState
    //{

    //    get { return isActiveState; }


    //    set
    //    {
    //        isActiveState = value;


    //    }
    //}



    public override void StateSetup(AIBase aAIscript)
    {
        this.aiScript = aAIscript;

        //idleTimeUntil = 5;

        Debug.Log("Setting Up idle state State");
    }

    public override void OnEnterState()
    {

        Debug.Log("In idle State");
        //IsActiveState = true;

        //StartCoroutine(ChangeToPatrol(idleTimeUntil));

        return;
    }

    public override void OnExitState()
    {
        //StopCoroutine(ChangeToPatrol(idleTimeUntil));

        //IsActiveState = false;

        Debug.Log("Exiting idle State");

        return;
    }

    public override void ChangeState(BaseStateClass aNewState, ref BaseStateClass aCurrState)
    {
        Debug.Log("Changing from Idle");
        aCurrState.OnExitState();


        aNewState.OnEnterState();

        return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("idle functionality");

        //StartCoroutine(ChangeToPatrol(idleTimeUntil));
    }


    public override void UpdateNewTarget(Transform aNewTarget)
    {

    }


    //public override void OnEveryFrame()
    //{
    //    //For Checking for player

    //    Debug.Log("CheckingForPlayer");
    //}




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //OnEveryFrame();
    }
}
