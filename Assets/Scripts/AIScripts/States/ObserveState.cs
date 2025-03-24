using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class ObserveState : BaseStateClass
{
    public TargetScript observedTarget;

    Transform currPosition;

    public ObserveState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
        Debug.Log("Entering Observe State");

        observedTarget = aiScript.searchFunctionSettings.playerObj;

        currPosition = aiScript.gameObject.transform;

        aiScript.agent.isStopped = true;

        return;
    }

    /// <summary>
    /// Will look at the player if they are within range, and will switch to search state when the player moves far enough.
    /// </summary>
    public override void CurrStateFunctionality()
    {
        Debug.Log("Observing Player");

        aiScript.transform.LookAt(observedTarget.transform, Vector3.up);

        if (Vector3.Distance(observedTarget.transform.position, currPosition.position) >= aiScript.observeSettings.maxObserveDistance)
            {
                Debug.Log("Player out of range");
                aiScript.SwitchStates(StateMachineEnum.Search);
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
