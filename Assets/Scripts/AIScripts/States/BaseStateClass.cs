using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseStateClass : MonoBehaviour
{
    protected bool isActiveState;

    public abstract bool IsActiveState
    {
        get;

        set;
    }

    public AIBase aiScript;

    public virtual void StateSetup(AIBase aAIscript)
    {
        this.aiScript = aAIscript;
    }
    public abstract void OnEnterState();

    public abstract void CurrStateFunctionality();

    public abstract void OnExitState();

    public abstract void ChangeState(BaseStateClass aNewState);

    public virtual void DeactivateState()
    {
        this.enabled = false;
    }

    protected void OnEnable()
    {
        OnEnterState();
    }

}
