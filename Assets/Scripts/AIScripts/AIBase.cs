using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.XR;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class AIBase : MonoBehaviour
{




    

    public float stateSwitchTimer;

    public float searchTimer;
   

    [Header("Idle Info")]

    public int idleTimeUntil;

    public IdleState idle = null;

    [Header("Patrol Info")]

    public PatrolState patrol = null;

    public Transform[] PatrolDestinations;

    public Transform CurrPatrolDestination;

    [Header("Interact Info")]

    private float targetInteractDistance;

    public float TargetInteractDistance{

        get { return targetInteractDistance; }
        set { TargetInteractDistance = value;

            agent.stoppingDistance = value;
        }
    }

    public InteractState interact = null;

    public GameObject PrevTarget;

    public GameObject CurrTarget;

    [HideInInspector]
    public List<GameObject> TargetsBacklog; 

    [Header("Searching For Player Info")]

    public Transform Eyes;

    public float radius;

    [Range(0, 360)]
    public float angle;

    public LayerMask TargetMask;

    public LayerMask EnvironmentMask;

    private bool isBusywithTarget;

    private bool targetFound;

    


    [Header("Chase Info")]

   public ChaseState chase = null;

    public float ChaseRange;
    

    private float speed;

    public float Speed
    {
        get { return speed; } 
        set { speed = value; 

            agent.speed = value;
        }
    }

    [Header("Searching Info")]

    SearchState search = null;

    public float rotateSpeed;

    

    [HideInInspector]
    public bool TargetFound
    {
        get { return targetFound; }

        set { targetFound = value;

            if (targetFound)
            {
                isBusywithTarget = true;
                SwitchStates(currActiveState, interact);
                TargetFound = false;
            }
                
            

        }
    }

    [Header("Navigation Info")]

    public NavMeshAgent agent;

    [Header("State Machine Info")]

    private AIBase thisAIObj;

    public BaseStateClass currActiveState;

    BaseStateClass previousState;

public void SwitchStates(BaseStateClass aCurrActiveState, BaseStateClass aNextState)
    {

        if (stateSwitchTimer > 0)
        {
            return;
        }

            Debug.Log("New State Decision");
            if (aNextState == aCurrActiveState)
            {
                return;
            }

            currActiveState.ChangeState(aNextState);

            //previousState = aCurrActiveState;

            currActiveState = aNextState;

    }

    public void ReturnToPreviousState()
    {
        SwitchStates(currActiveState, previousState);
    }

    public bool UpdateNewTargets()
    {
        return true;
    }

    void Awake()
    {
        thisAIObj = GetComponent<AIBase>();

        agent = GetComponent<NavMeshAgent>();

        idle = gameObject.AddComponent<IdleState>();

        patrol = gameObject.AddComponent<PatrolState>();

        interact = gameObject.AddComponent<InteractState>();

        chase = gameObject.AddComponent<ChaseState>();

        search = gameObject.AddComponent<SearchState>();

        idle.StateSetup(thisAIObj);

        patrol.StateSetup(thisAIObj);

        interact.StateSetup(thisAIObj);

        chase.StateSetup(thisAIObj);

        search.StateSetup(thisAIObj);

        TargetsBacklog = new List<GameObject>();

}

    void OnEnable()
    {
        CurrTarget = null;

        TargetFound = false;

        isBusywithTarget = false;

        currActiveState = idle;

        StartCoroutine(ChangeToPatrol(idleTimeUntil));
    }

    // Update is called once per frame
    void Update()
    {
        currActiveState.CurrStateFunctionality();

        stateSwitchTimer -= Time.deltaTime;

        searchTimer -= Time.deltaTime;

    }

    /// <summary>
    /// 
    /// Given a list of GameObject tags to look out for, cast a ray from Eyes.position to a specific range ahead of them, and return the gameObject that possesses the correct tag.
    /// 
    /// The player will then move towards that gameObject, and, then, when close enough to the gameObject
    /// 
    /// </summary>
    /// <returns></returns>
    public void SearchForTargets()
    {
        if(searchTimer > 0)
        {
            return;
        }
        Debug.Log("Searching for targets");

        GameObject target;
        
        Collider[] AIRange = Physics.OverlapSphere(transform.position, radius, TargetMask);

        if (AIRange.Length != 0)
        {
            
            Debug.Log("Target found");

            for (int i = 0; i < AIRange.Length; i++)
            {
                target = AIRange[i].gameObject;

                if (target == CurrTarget || target == PrevTarget)
                {
                    Debug.Log("target is already current target or is a previous target");
                    return;
                }

                Vector3 directionToTarget = (target.gameObject.transform.position - Eyes.transform.position).normalized;

                if (Vector3.Angle(Eyes.transform.forward, directionToTarget) < angle / 2)
                {
                    
                    float distanceToTarget = Vector3.Distance(transform.position, target.gameObject.transform.position);

                    if (!Physics.Raycast(Eyes.transform.position, directionToTarget, distanceToTarget, EnvironmentMask))
                    {
                        
                        Debug.Log("Target seen");

                        bool isTargetValid = CurrentTargetAnalysis(target);

                        if (isTargetValid)
                        {
                            TargetFound = true;
                            
                        }
                            return;
                        

                    }
                  
                }
                
            }

        }

    }

    


    public void BaseTargetInteractFunction()
    {
        Debug.Log("Test Interact with target");
        StartCoroutine(BaseTargetTime());
        StopCoroutine(BaseTargetTime());

        return;
    }

    IEnumerator BaseTargetTime()
    {
        yield return new WaitForSeconds(5);

        StateComplete();

        Debug.Log("Target task is complete");

        yield break;
    }

    IEnumerator PlayerTargetTime()
    {
        yield return new WaitForSeconds(5);

        StateComplete();

        Debug.Log("Player task is complete");

        yield break;
    }

    public void PlayerInteractFunction()
    {
        StartCoroutine(PlayerTargetTime());
        StopCoroutine(PlayerTargetTime());

        return;
    }

    IEnumerator LPTargetTime()
    {
        yield return new WaitForSeconds(5);

        StateComplete();

        Debug.Log("Low Priority task is complete");

        yield break;
    }

    public void LowPriInteractFunction()
    {
        StartCoroutine(LPTargetTime());
        StopCoroutine(LPTargetTime());

        return;
    }

    public void StateComplete()
    {
        Debug.Log("Task Complete");

        TargetFound = false;

        isBusywithTarget = false;

        return;
    }

    private bool CurrentTargetAnalysis(GameObject aTarget)
    {

       

            //Starting case, the first target spotted, will be the target regardless of status
            if (CurrTarget == null || !isBusywithTarget)
            {
                Debug.Log("New object is set, proceed with interact state");

                CurrTarget = aTarget;
                
                interact.SetTarget(CurrTarget);
                

                return true;
            }

       return false;
    }

    public IEnumerator ChangeToPatrol(int aIdleTime)
    {

        Debug.Log("Waiting to change to patrol");

        yield return new WaitForSeconds(aIdleTime);

        SwitchStates(idle, patrol);

    }

  

    public GameObject RetrieveCurrTarget()
    {
        return CurrTarget;
    }

    internal void LostTarget()
    {
        SwitchStates(currActiveState, search);
    }
}
