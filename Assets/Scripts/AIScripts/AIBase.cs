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

    public GameObject CurrTarget;
    //enum TargetTypes
    //{
    //    ActiveObject,
    //    InactiveObject,
    //    Player,
    //    Target
    //}

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


    [Header("Navigation Info")]

    public NavMeshAgent agent;


    [Header("State Machine Info")]

    private AIBase thisAIObj;

    public BaseStateClass currActiveState;

    BaseStateClass previousState;








   






public void SwitchStates(BaseStateClass aCurrActiveState, BaseStateClass aNextState)
    {
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

        idleTimeUntil = 5;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        currActiveState = idle;

        StartCoroutine(ChangeToPatrol(idleTimeUntil));
    }

    // Update is called once per frame
    void Update()
    {

        currActiveState.CurrStateFunctionality();

        CurrTarget = SearchForTargets();

        if(CurrTarget != null)
        {
            SwitchStates(currActiveState, interact);

            interact.SetTargetLocation(CurrTarget);

        }

        //SearchForTargets();
    }

    /// <summary>
    /// 
    /// Given a list of GameObject tags to look out for, cast a ray from Eyes.position to a specific range ahead of them, and return the gameObject that possesses the correct tag.
    /// 
    /// The player will then move towards that gameObject, and, then, when close enough to the gameObject
    /// 
    /// </summary>
    /// <returns></returns>
    public GameObject SearchForTargets()
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

                targetTag = target.tag;

                Vector3 directionToTarget = (target.gameObject.transform.position - Eyes.transform.position).normalized;

                if (Vector3.Angle(Eyes.transform.forward, directionToTarget) < angle / 2)
                {
                    //set agent destination to new found target.

                    float distanceToTarget = Vector3.Distance(transform.position, target.gameObject.transform.position);

                    if (!Physics.Raycast(Eyes.transform.position, directionToTarget, distanceToTarget, EnvironmentMask))
                    {
                        Debug.Log("Target seen, moving to target");

                        if (CurrTarget == null)
                        {
                            return target;
                        }
                        else if (CurrTarget != null)
                            CurrentTargetAnalysis(target);

                        
                    }
                    else
                    {
                        Debug.Log("Target not found in front of AI.");
                        continue;
                    }
                }
                else
                {
                    Debug.Log("Maintain current Patrol");
                    continue;
                }
            }




        }
        else
            Debug.Log("Continue current State");
            return null;
        
            

        //Debug.DrawRay(Eyes.position, (TargetObjRef.transform.position - Eyes.position).normalized * VisionRange, Color.red, 0.01f);

        ////don't check for the player if they are behind the player, aka -1 for dot product
        //if (Vector3.Dot((target.position - eyes.position).normalized, eyes.forward) < 0)
        //{
        //    return false;
        //}
        ////not normalized, as we need a set distance that isn't one


        ////a raycast from the eyes toward the player, able to go through glass and other enemies, up to MaxPlayerDistance
        ////and stores the hit in a variable

        ////origin, direction, stores hit properties, max distance of raycast, objects that can obstruct raycast
        //if (Physics.Raycast(Eyes.position, (target.position - eyes.position).normalized,
        //    out RaycastHit hit, visionRange, visionBlockers))
        //{
        //    if (hit.transform.CompareTag(playerTag))
        //    {
        //        return true;
        //    }
        //}

        ////if(Physics.Raycast(eyes.position,(target.position - eyes.position).normalized, out RaycastHit hit, 
        ////visionRange, 
        //return false;



       
    }

    private void CurrentTargetAnalysis(GameObject aTarget)
    {
       
    }

    IEnumerator ChangeToPatrol(int aIdleTime)
    {

        Debug.Log("Waiting to change to patrol");

        yield return new WaitForSeconds(aIdleTime);

        SwitchStates(idle, patrol);

    }

}
