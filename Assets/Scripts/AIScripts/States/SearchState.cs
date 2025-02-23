using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseStateClass
{
    float timer;

    string aISearchMethod;

    public bool searchingForPlayer;

    bool heardSomething;

    Vector3 detectedLocation;

    float angle;

    public SearchState(AIBase aAIscript) : base(aAIscript)
    {
        this.aiScript = aAIscript;
    }
    public override void OnEnterState()
    {
        Debug.Log("In Search State");

        timer = aiScript.searchStateVal;

        aISearchMethod = System.Enum.GetName(typeof(AIBase.SearchMethod), aiScript.searchMethod);

        

        return;
    }

    public override void CurrStateFunctionality()
    {
        timer -= Time.deltaTime;

        Debug.Log("Search Functionality");
        if (aiScript.SearchForTargets())
        {
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.playerDetected);
        }

        if (timer <= 0)
        {
            aiScript.SwitchStates(aiScript.currActiveState, aiScript.patrol);
        }

        if(searchingForPlayer)
        {
            switch(aISearchMethod)
            {
                case "SearchInPlace":
                    {
                        SearchInPlaceFunction();
                        break;
                    }
                case "SearchInRandomPoint":
                    {
                        SearchRandomPointFunction();
                        break;
                    }
            }
        }


    }

    public void SearchInPlaceFunction()
    {
        Debug.Log("Searching in Place");

        angle = Mathf.MoveTowardsAngle(aiScript.transform.eulerAngles.y, aiScript.rotatePosition, aiScript.rotateSpeed * Time.deltaTime);

        aiScript.transform.eulerAngles = new Vector3(aiScript.transform.eulerAngles.x, angle, aiScript.transform.eulerAngles.z);
    }
   
    public void SearchRandomPointFunction()
    {
        Debug.Log("Searching Random Point in Range");
    }

    public void MoveToPointFunction()
    {
        Debug.Log("Investigating Point");
    }

    public override void OnExitState()
    {
        Debug.Log("Exiting Search State");

        searchingForPlayer = false;
    }

    public override void ChangeState(BaseStateClass aNewState)
    {
        Debug.Log("Changing from Search State");

        aNewState.OnEnterState();

        OnExitState();
    }

    
}
