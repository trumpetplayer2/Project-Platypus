using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseStateClass
{
    public override bool IsActiveState
    {
        get { return isActiveState; }

        set
        {
            isActiveState = value;

            if (!IsActiveState)
                DeactivateState();

        }

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
