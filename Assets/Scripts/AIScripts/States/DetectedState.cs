using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectedState : BaseStateClass
{
    Transform currentTarget;

    public override void StateSetup(AIBase aAIscript)
    {
        this.aiScript = aAIscript;

        //idleTimeUntil = 5;

        Debug.Log("Setting Up idle state State");
    }

    public void StateSetup(Transform NewTarget)
    {
        currentTarget = NewTarget;
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
        Debug.Log("Changing from Detected");
        aCurrState.OnExitState();


        aNewState.OnEnterState();

        return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("idle functionality");

        //StartCoroutine(ChangeToPatrol(idleTimeUntil));
    }


   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
