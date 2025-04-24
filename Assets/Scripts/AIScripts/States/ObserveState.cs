using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class ObserveState : BaseStateClass
{
    public TargetScript observedTarget;

    Transform currPosition;

   

    float timer = 0f;

    public float timerMax;

    public Vector3 start;

    public Vector3 goal;


    public ObserveState(StateMachineInfo.AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }

    public override void OnEnterState()
    {
       

        observedTarget = aiScript.searchFunctionSettings.playerObj;

        currPosition = aiScript.gameObject.transform;

        aiScript.agent.isStopped = true;

        timerMax = aiScript.observeSettings.observeTimerMax;

        aiScript.aIAnimator.SetBool("Walk", false);

        aiScript.aIAnimator.SetBool("Interact", true);

        return;
    }

    /// <summary>
    /// Will look at the player if they are within range, and will switch to search state when the player moves far enough.
    /// </summary>
    public override void CurrStateFunctionality()
    {
        
       

        if(goal == null || timer >= timerMax)
        {
            start = aiScript.transform.forward;

            goal = (observedTarget.transform.position - aiScript.transform.position).normalized;

            timer = 0f;


        }

        timer += Time.deltaTime;

        aiScript.transform.forward = Vector3.Lerp(start, goal, timer / timerMax);

        ////aiScript.transform.LookAt(observedTarget.transform, Vector3.up);

        //rotation = Quaternion.LookRotation(observedTarget.transform.position - aiScript.transform.position.normalized).eulerAngles;

        ////rotation.y = 0f;

        //Vector3.Lerp(aiScript.transform.forward, rotation, )

      

        //rotation *= 2;

        //currPosition.rotation = Quaternion.Euler(rotation);

        if (Vector3.Distance(observedTarget.transform.position, currPosition.position) >= aiScript.observeSettings.maxObserveDistance)
            {
               
                aiScript.SwitchStates(StateMachineEnum.Search);
                return;
            }

    }

    public override void OnExitState()
    {
       

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
