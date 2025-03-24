using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class BaseStateClass
{

    public StateMachineInfo.AIBase aiScript;

    public BaseStateClass(StateMachineInfo.AIBase aAIscript) { 
    
        this.aiScript = aAIscript;

    }

    public abstract void OnEnterState();

    public abstract void CurrStateFunctionality();

    public abstract void OnExitState();

    public abstract void ChangeState(BaseStateClass aNewState);

   

}
