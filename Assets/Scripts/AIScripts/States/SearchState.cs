using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseStateClass
{
    
    
    public SearchState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }
    public override void OnEnterState()
    {
        Debug.Log("In Search State");


    }

    public override void CurrStateFunctionality()
    {
        Debug.Log("Search Functionality");
        aiScript.SearchForTargets();

        


    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Search State");
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        OnExitState();
    }

    
}
