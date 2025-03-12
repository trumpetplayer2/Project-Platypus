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


        observedTarget = aiScript.searchFunctionSettings.CurrTarget == null ? null : aiScript.searchFunctionSettings.CurrTarget;

        timer = 0;

        maxDistance = aiScript.observeSettings.maxObserveDistance;

        currPosition = aiScript.gameObject.transform;

        angleBetween = Vector3.Angle(aiScript.transform.position, observedTarget.transform.position);

        aiScript.agent.isStopped = true;

        return;
    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("Observing Player");

        //update player state and reset timer
        if (aiScript.SearchForTargets() == DetectedType.Player)
        {
            timer = 0;
            float eulerZ = aiScript.transform.rotation.eulerAngles.z;
            aiScript.transform.LookAt(observedTarget.transform, Vector3.up);
            aiScript.transform.rotation = Quaternion.Euler(aiScript.transform.rotation.eulerAngles.x, aiScript.transform.rotation.eulerAngles.y, eulerZ);
            return;
        }
        
            timer += Time.deltaTime;

            float angle = Mathf.Lerp(aiScript.transform.rotation.eulerAngles.y, angleBetween, timer / 0.5f);

            currPosition.transform.Rotate(new Vector3(0, angle, 0));

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

        return;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        aNewState.OnEnterState();

        OnExitState();

        return;
    }
}
