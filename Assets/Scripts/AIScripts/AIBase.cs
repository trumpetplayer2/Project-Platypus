using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.XR;
using System.Net;

public class AIBase : MonoBehaviour
{

    [Header("Idle Info")]

    public int idleTimeUntil;

    public IdleState idle = null;


    [Header("Patrol Info")]

    public PatrolState patrol = null;

    public Transform[] PatrolDestinations;

    public Transform CurrPatrolDestination;


   


    [Header("Interact Info")]

    public float TargetInteractDistance;


    public InteractState interact = null;

    public GameObject PrevTarget;

    public GameObject CurrTarget;
    enum TargetTypes
    {
        ActiveObject,
        InactiveObject,
        Player,
        Target
    }

    [Header("Searching For Player Info")]

   

    public Transform Eyes;

    public float VisionRange;

    public GameObject PlayerObjRef;

    public GameObject TargetObjRef;

    public GameObject ObjectObjRef;


    public float radius;

    [Range(0, 360)]
    public float angle;


    public LayerMask TargetMask;

    public LayerMask EnvironmentMask;

    private bool isBusywithTarget;

    private bool targetFound;

    

    [HideInInspector]
    public bool TargetFound
    {
        get { return targetFound; }

        set { targetFound = value;


            //Debug.Log(targetFound = true ? "target is found" : "target is not found");

            if (!targetFound  && !isBusywithTarget && currActiveState == interact)
            {
                Debug.Log("Returning to idle state");
                targetFound = false;
                isBusywithTarget = false;
                SwitchStates(currActiveState, idle);
            }

            if (targetFound || CurrTarget != null)
            {
                Debug.Log("switching to interact state");
                SwitchStates(currActiveState, interact);

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
        if(aNextState == aCurrActiveState)
        {
            return;
        }
        Debug.Log("New State Decision");

        currActiveState.ChangeState(aNextState, ref aCurrActiveState);

        previousState = aCurrActiveState;

        currActiveState = aNextState;
    }

    public void ReturnToPreviousState()
    {
        SwitchStates(currActiveState, previousState);
    }


    //will search for specific game objects, such as the player, misplaced objects, and specific task related objects.
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


        idle.StateSetup(thisAIObj);

        patrol.StateSetup(thisAIObj);

        interact.StateSetup(thisAIObj);

        
    }

    // Start is called before the first frame update
    void Start()
    {

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

        
        

        

        SearchForTargets();
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
        

        Debug.Log("Searching for targets");

        GameObject target;

        string targetTag;
        //establishes detection radius
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
                    continue;
                }

                targetTag = target.tag;

                Vector3 directionToTarget = (target.gameObject.transform.position - Eyes.transform.position).normalized;

                if (Vector3.Angle(Eyes.transform.forward, directionToTarget) < angle / 2)
                {
                    //set agent destination to new found target.

                    float distanceToTarget = Vector3.Distance(transform.position, target.gameObject.transform.position);

                    if (!Physics.Raycast(Eyes.transform.position, directionToTarget, distanceToTarget, EnvironmentMask))
                    {
                        
                        Debug.Log("Target seen");
                     
                        if (target != CurrTarget)
                        {
                            Debug.Log("Sending Target for analysis");
                            
                            CurrentTargetAnalysis(target);
                            continue;
                        }
                        else if (target == null)
                        {
                            Debug.Log("target is null");
                            continue;
                        }
                       



                    }
                    else
                    {
                        TargetFound = false;
                        Debug.Log("Target not found in front of AI.");
                        continue;
                    }
                }
                else
                {
                    TargetFound = false;
                    Debug.Log("Maintain current Patrol");
                    continue;
                }
            }




        }
        else
            TargetFound = false;
            Debug.Log("Continue current State");
        return;
        
            

      
       
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
        PrevTarget = CurrTarget;

        CurrTarget = null;

        TargetFound = false;

        isBusywithTarget = false;

        return;
    }

    private void CurrentTargetAnalysis(GameObject aTarget)
    {

        Debug.Log(targetFound);

        Debug.Log("Analyzing potential target");

        Debug.Log("Is the target null?");
        if(aTarget != null)
        {
            Debug.Log("Given argument is not null");
            
            //Starting case, the first target spotted, will be the target regardless of status
            if (CurrTarget == null)
            {
                Debug.Log("New object is set, proceed with interact state");

                CurrTarget = aTarget;
                interact.SetTarget(CurrTarget);
                TargetFound = true;
                isBusywithTarget = true;

                return;
            }
            else if(CurrTarget != null && isBusywithTarget)
            {
                if (aTarget.gameObject.CompareTag("Player"))
                {
                    PrevTarget = CurrTarget;

                    CurrTarget = aTarget;

                    interact.SetTarget(CurrTarget);

                    TargetFound = true;

                    isBusywithTarget = true;
                }
                else if (aTarget.gameObject.CompareTag("Target")){

                    PrevTarget = CurrTarget;
                    CurrTarget = aTarget;
                    interact.SetTarget(CurrTarget);
                   

                    TargetFound = true;

                    isBusywithTarget = true;
                }
                else if (aTarget.gameObject.CompareTag("LowestPriority"))
                {
                    PrevTarget = CurrTarget;
                    CurrTarget = aTarget;
                    interact.SetTarget(CurrTarget);

                   
                }

            }
            
            //}
            //else
            //{
            //    Debug.Log("busy with target");
            //    return;
            //}

            
        }
        else
        {
            TargetFound = false;
            Debug.Log("Given argument is null");
            return;
        }
       
        
    }

    

    public IEnumerator ChangeToPatrol(int aIdleTime)
    {

        Debug.Log("Waiting to change to patrol");

        yield return new WaitForSeconds(aIdleTime);

        SwitchStates(idle, patrol);

    }

    

   
}
