using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseStateClass : MonoBehaviour
{
    protected bool isActiveState;

    public virtual bool IsActiveState
    {
        get;

        set;



    }


    public AIBase aiScript;


    public virtual void StateSetup(AIBase aAIscript)
    {
        this.aiScript = aAIscript;
    }
    //public BaseStateClass(AIBase aAIscript)
    //{
    //    this.aiScript = aAIscript;
    //}


    // protected bool isInCoroutine { get; set; }

    public abstract void OnEnterState();

    public abstract void CurrStateFunctionality();

    public abstract void OnExitState();

    public abstract void ChangeState(BaseStateClass aNewState, ref BaseStateClass aCurrState);

    //public abstract void OnEveryFrame();

   
    



    //protected IEnumerator ChangeStateCoroutine(int aWaitForSeconds, BaseStateClass aNextState)
    //{
    //    yield return new WaitForSeconds(aWaitForSeconds);



    //}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
