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

[System.Serializable]

public class IdleItems
{

}
public class AIBase : MonoBehaviour
{

    [Header("Initial State")]

    public InitialState initial = null;

    [Header("Idle Info")]

    public IdleState idle = null;

    public int idleTimeUntil;

    [Header("Patrol Info")]

    public PatrolState patrol = null;

    public Transform[] PatrolDestinations;

    public Transform CurrPatrolDestination;

    [Header("Interact Info")]

    public InteractState interact = null;

    public TargetScript CurrTarget;

    public TargetScript playerObj;

    [Header("Chase Info")]

    public ChaseState chase = null;

    public float speedVal;

    public float Speed
    {
        get { return speed; }
        set
        {
            speed = value;

            agent.speed = speed;
        }
    }

    private float speed;

    public float chaseSpeedVal;

    public float chaseMaxDistance;

    public float chaseMinDistance;

    public float losingTargetVal;

    public float catchTimerVal;

    [Header("Searching Info")]

    public SearchState search = null;

    public float searchStateVal;

    public enum SearchMethod
    {
        SearchInPlace,
        SearchInRandomPoint
    }

    public SearchMethod searchMethod;

    public float rotateSpeed;

    public float rotatePosition;

    [Header("Player Detected Info")]

    public PlayerDetectedState playerDetected = null;

    public bool playerFound;

    public enum AIResponse
    {
        Chase,
        Observe

    }

    public AIResponse SetPlayerResponse;

    [Header("SearchingForTargets Info")]

    public Transform Eyes;

    public float radius;

    [Range(0, 360)]
    public float angle;

    public LayerMask TargetMask;

    public LayerMask EnvironmentMask;

    [Header("Navigation Info")]

    public NavMeshAgent agent;

    public float distanceBetweenTarget;

    [Header("State Machine Info")]

    public BaseStateClass currActiveState;

    public float stateSwitchTimerVal;

    public float searchTimerVal;

    private float stateSwitchTimer;

    private float searchTimer;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        initial = new InitialState(this);

        idle = new IdleState(this);

        patrol = new PatrolState(this);

        interact = new InteractState(this);

        chase = new ChaseState(this);

        search = new SearchState(this);

        playerDetected = new PlayerDetectedState(this);

        Speed = speedVal;

        CurrTarget = null;
    }

    void OnEnable()
    { 
        currActiveState = initial;

    }

    // Update is called once per frame
    void Update()
    {
        currActiveState.CurrStateFunctionality();

        stateSwitchTimer -= Time.deltaTime;

        if (stateSwitchTimer <= 0)
            stateSwitchTimer = stateSwitchTimerVal;

        searchTimer -= Time.deltaTime;

        if (searchTimer <= 0)
            searchTimer = searchTimerVal;

    }

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

        //stateSwitchTimer = stateSwitchTimerVal;

    }

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

            playerFound = true;
            return false;
        }

        //Starting case, the first target spotted, will be the target regardless of status
        if (CurrTarget == null || !aTarget.TargetInfo.wasCompleted)
        {
            CurrTarget = aTarget;

            Debug.Log("New object is set, proceed with interact state");

            return true;
        }

       
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
