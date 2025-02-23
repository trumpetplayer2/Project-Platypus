using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseStateClass
{

    public bool playerDetected;

    public AIBase aiScript;

    public BaseStateClass(AIBase aAIscript) { 
    
        this.aiScript = aAIscript;

    }

    public abstract void OnEnterState();

    public abstract void CurrStateFunctionality();

    public abstract void OnExitState();

    public abstract void ChangeState(BaseStateClass aNewState);

    public void ChangeToInteract()
    {
        aiScript.SwitchStates(aiScript.currActiveState, aiScript.interact);
    }

  
 

}
