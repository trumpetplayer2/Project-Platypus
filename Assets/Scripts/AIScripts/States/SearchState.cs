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

    }

    public override void CurrStateFunctionality()
    {
        aiScript.SearchForTargets();


    }

    public override void OnExitState()
    {
       
    }

    public override void ChangeState(BaseStateClass aNewState)
    {

    }

    
}
