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
    [Header("Idle Info")]

    public int idleTimeUntil;

    public IdleState idle = null;

    [Header("Patrol Info")]

    public PatrolState patrol = null;

    public Transform[] PatrolDestinations;

    public Transform CurrPatrolDestination;

    [Header("Interact Info")]

    public float targetInteractDistanceVal;

    private float targetInteractDistance;

    public float TargetInteractDistance{

        get { return targetInteractDistance; }
        set { TargetInteractDistance = value;

            agent.stoppingDistance = value;
        }
    }

    public InteractState interact = null;

    public TargetScript PrevTarget;

    public  TargetScript CurrTarget;

    public TargetScript playerObj;

   
    public List<TargetScript> TargetsBacklog; 

    [Header("Searching For Player Info")]

    public Transform Eyes;

    public float radius;

    [Range(0, 360)]
    public float angle;

    public LayerMask TargetMask;

    public LayerMask EnvironmentMask;

    private bool targetFound;

    [Header("Chase Info")]

    public ChaseState chase = null;

    public float ChaseRange;

    public float speedVal;

    public float Speed
    {
        get { return speed; } 
        set { speed = value; 

            agent.speed = speed;
        }
    }
   
    private float speed;
    [Header("Searching Info")]


    public float LookingSpeed;

    public SearchState search = null;



    public bool TargetFound
    {
        get { return targetFound; }

        set { targetFound = value;

           

        }
    }

    [Header("Navigation Info")]

    public NavMeshAgent agent;

    
   

    [Header("State Machine Info")]

   [SerializeField] public BaseStateClass currActiveState;

    public float stateSwitchTimerVal;

    public float searchTimerVal;

    BaseStateClass previousState;

    private float stateSwitchTimer;

    private float searchTimer;


    public void SwitchStates(BaseStateClass aCurrActiveState, BaseStateClass aNextState)
    {

        if (stateSwitchTimer > 1)
        {
            Debug.Log("Exiting SwitchStates Function");
            return;
        }

            Debug.Log("New State Decision");
            if (aNextState == aCurrActiveState)
            {
                return;
            }

            currActiveState.ChangeState(aNextState);

            currActiveState = aNextState;

        stateSwitchTimer = stateSwitchTimerVal;

    }

   
    
    void Awake()
    {
        

        agent = GetComponent<NavMeshAgent>();

        idle = new IdleState(this);

       

        patrol = new PatrolState(this);


        interact = new InteractState(this);

        

        chase = new ChaseState(this);

       

        search = new SearchState(this);

        

        TargetsBacklog = new List<TargetScript>();

        Speed = speedVal;

        

}

    void OnEnable()
    {
        CurrTarget = null;

        TargetFound = false;

        

        currActiveState = idle;

    }

    // Update is called once per frame
    void Update()
    {
        currActiveState.CurrStateFunctionality();

        stateSwitchTimer -= Time.deltaTime;

        if (stateSwitchTimer <= 0)
            stateSwitchTimer = stateSwitchTimerVal;

        searchTimer -= Time.deltaTime;

        if(searchTimer <= 0)
            searchTimer = searchTimerVal;

    }

    /// <summary>
    /// 
    /// Given a list of GameObject tags to look out for, cast a ray from Eyes.position to a specific range ahead of them, and return the gameObject that possesses the correct tag.
    /// 
    /// The player will then move towards that gameObject, and, then, when close enough to the gameObject
    /// 
    /// </summary>
    /// <returns></returns>
    public bool SearchForTargets()
    {
        if(searchTimer > 1)
        {
            Debug.Log("Search Cooldown");
            return false;
        }

      
        Debug.Log("Searching for targets");

  
        
        Collider[] AIRange = Physics.OverlapSphere(transform.position, radius, TargetMask);
        

        if (AIRange.Length != 0)
        {
            
            Debug.Log("Target found");

            for (int i = 0; i < AIRange.Length; i++)
            {

                if (!AIRange[i].TryGetComponent<TargetScript>(out TargetScript target))
                {
                    continue;
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
                            Debug.Log("Target is Valid, returning true");
                           
                            return true;
                           
                        }
                        else
                        {
                            return false;
                        }
                        

                    }
                  
                }
                return false;
            }

        }
        return false;

    }

    private bool CurrentTargetAnalysis(TargetScript aTarget)
    {
        if (IsPlayer(aTarget))
        {
            Debug.Log("Player Detected");
            playerObj = aTarget;
            return true;
        }

        //Starting case, the first target spotted, will be the target regardless of status
        if (CurrTarget == null || !aTarget.TargetInfo.wasCompleted)
        {
            CurrTarget = aTarget;

            Debug.Log("New object is set, proceed with interact state");

            return true;
        }

        TargetsBacklog.Add(aTarget);
       return false;
    }

    public bool IsPlayer(TargetScript aTarget)
    {
        if (aTarget.CompareTag("Player"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public TargetScript RetrieveCurrTarget()
    {
        if(playerObj != null)
        {
            return playerObj;
        }
        else
        {
            return CurrTarget;
        }
       
    }

    
   
}
