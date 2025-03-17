using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class ObserveState : BaseStateClass
{
    public TargetScript observedTarget;

   

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


        observedTarget = aiScript.searchFunctionSettings.playerObj;
        
        

        maxDistance = aiScript.observeSettings.maxObserveDistance;

        currPosition = aiScript.gameObject.transform;

        angleBetween = Vector3.Angle(aiScript.transform.position, observedTarget.transform.position);

        aiScript.agent.isStopped = true;

        return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("Observing Player");

        aiScript.transform.LookAt(observedTarget.transform, Vector3.up);

        if (Vector3.Distance(observedTarget.transform.position, currPosition.position) >= aiScript.observeSettings.maxObserveDistance)
            {
                Debug.Log("Player out of range");
                aiScript.SwitchStates(aiScript.search);
                return;
            }

    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Observe State");

        observedTarget = null;

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
