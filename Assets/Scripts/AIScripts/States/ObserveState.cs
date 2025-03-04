using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObserveState : BaseStateClass
{
    public TargetScript observedTarget;

    float timer;

    Transform currPosition;

    float angleBetween;

    float maxDistance;

    //vecotr between ai and player

    //angle between ai and forward

    //lerp.angle to forward direction
    public ObserveState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {

        this.aiScript = aAIscript;

    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Observe State");

        timer = 0;

        maxDistance = aiScript.observeSettings.maxDistanceVal;

        observedTarget = aiScript.RetrieveCurrTarget();

        currPosition = aiScript.gameObject.transform;

        angleBetween = Vector3.Angle(aiScript.transform.position, observedTarget.transform.position);

        aiScript.agent.isStopped = true;

        return;
    }

    public override void CurrStateFunctionality()
    {
        //update player state and reset timer
        //if(aiScript.SearchForTargets())
        timer += Time.deltaTime;


        Debug.Log("Observing Player");

        float angle = Mathf.Lerp(aiScript.transform.rotation.eulerAngles.y, angleBetween , timer / 0.5f);

        currPosition.transform.Rotate(new Vector3(0, angle, 0));
       


        //if (Vector3.Distance(observedTarget.transform.position, currPosition.position) <= 1)
        //{
        //    Debug.Log("Next to player");
        //    currPosition.rotation = Quaternion.AngleAxis(angleBetween, currPosition.up);
        //    timer = aiScript.observeSettings.rotateTimerVal;
            
        //}
        //else
        //{
            
        //}

        if (Vector3.Distance(observedTarget.transform.position, currPosition.position) >= maxDistance)
        {
            Debug.Log("Player out of range");
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.search);
            return;
        }

        
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Observe State");

        observedTarget = null;

        aiScript.interactSettings.playerObj = null;

        timer = 1;

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        aNewState.OnEnterState();

        OnExitState();

        return;
    }
}
