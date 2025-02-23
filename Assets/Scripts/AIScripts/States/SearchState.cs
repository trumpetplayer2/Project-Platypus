using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseStateClass
{

    float timer;

    public SearchState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }
    public override void OnEnterState()
    {
        Debug.Log("In Search State");

        timer = aiScript.searchStateVal;

        return;
    }

    public override void CurrStateFunctionality()
    {
        timer -= Time.deltaTime;

        Debug.Log("Search Functionality");
        aiScript.SearchForTargets();

        if(timer <= 0)
        {
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.patrol);
        }


    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Search State");
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Changing from Search State");

        aNewState.OnEnterState();

        OnExitState();
    }

    
}
