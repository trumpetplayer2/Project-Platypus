using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public enum DetectedType
{
    None,
    Player,
    Object
}

public enum StateMachineEnum
{
    Idle,
    Patrol,
    Interact,
    Chase,
    Search,
    Observe
}

namespace StateMachineInfo
{

    [System.Serializable]
    public class IdleSettings
    {
        public int idleTimeUntil;
    }

    [System.Serializable]
    public class PatrolSettings
    {
        public Transform[] PatrolDestinations;

        [ReadOnly] public Transform CurrPatrolDestination = null;

        public float patrolDistanceToDestination;
    }

    [System.Serializable]
    public class InteractSettings
    {
        public float distanceBetweenTarget;
    }

    [System.Serializable]
    public class ChaseSettings
    {
        public float chaseSpeedIncrease;

        public float chaseMaxDistance;

        public float chaseMinDistance;

        public float losingTargetTime;

        public float catchTimerTimer;

        public enum ChaseSpeciality
        {
            Push,
            Grab
        }

        public ChaseSpeciality chaseSpeciality;
    }

    [System.Serializable]
    public class ObserveSettings
    { 
        public float maxObserveDistance;

        public float rotateSpeed;
    }

    [System.Serializable]
    public class SearchStateSettings
    { 
        public float searchStateTime;

        public enum SearchMethod
        {
            SearchInPlace,
            SearchInRandomPoint
        }

        public SearchMethod searchMethod;

        public Vector3 noiseLocation;

        public bool heardSomething;
    }

    [System.Serializable]
    public class PlayerDetectedSettings
    {
        public enum AIResponse
        {
            Chase,
            Observe

        }

        public AIResponse SetPlayerResponse;
    }

    [System.Serializable]
    public class SearchFunctionSettings
    {
        public Transform Eyes;

        public float radius;

        [Range(0, 360)]
        public float angle;

        public LayerMask TargetMask;

        public LayerMask EnvironmentMask;

        [ReadOnly] public TargetScript CurrTarget = null;


        [ReadOnly] public TargetScript playerObj = null;

        public int maxColliders;

    }

 
    public class AIBase : MonoBehaviour
    {
       [ReadOnly] public StateMachineEnum stateMachineEnum;

        #region StateSettings
        public IdleSettings idleSettings;

        public PatrolSettings patrolSettings;

        public InteractSettings interactSettings;

        public ChaseSettings chaseSettings;

        public SearchStateSettings searchStateSettings;

        public PlayerDetectedSettings playerDetectedSettings;

        public SearchFunctionSettings searchFunctionSettings;

        public ObserveSettings observeSettings;


        #endregion

        #region State Objs
        public InitialState initial = null;

        public IdleState idle = null;

        public PatrolState patrol = null;

        public InteractState interact = null;

        public ChaseState chase = null;

        public SearchState search = null;

        public ObserveState observe = null;

        public PlayerDetectedState playerDetected = null;

        #endregion

        public NavMeshAgent agent;

        public BaseStateClass currActiveState;

        public float stateSwitchTime;

        public float aIDetectionTime;

        [ReadOnly] public float stateSwitchTimer;

        [ReadOnly] public float searchTimer;

        public float speedVal;

        Collider[] AIRange;

        //public float Speed
        //{
        //    get { return speed; }
        //    set
        //    {
        //        speed = value;

        //        agent.speed = speed;
        //    }
        //}

        //private float speed;


        bool switchCooldown;
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

            observe = new ObserveState(this);

            //Speed = speedVal;

            agent.speed = speedVal;

            AIRange = new Collider[searchFunctionSettings.maxColliders];
        }

        void OnEnable()
        {
            currActiveState = initial;

        }

        // Update is called once per frame
        void Update()
        {
            currActiveState.CurrStateFunctionality();

            if(switchCooldown)
            {
                stateSwitchTimer -= Time.deltaTime;

                if (stateSwitchTimer <= 0)
                {
                    stateSwitchTimer = stateSwitchTime;
                    switchCooldown = false;
                }
                        
            }

            searchTimer -= Time.deltaTime;

            if (searchTimer <= 0)
                searchTimer = aIDetectionTime;

        }

        public void SwitchStates(BaseStateClass aNextState)
        {
            if (switchCooldown)
            {
                Debug.Log("Exiting SwitchStates Function");
                return;
            }

            Debug.Log("New State Decision");
            if (aNextState == currActiveState)
            {
                return;
            }

            currActiveState.ChangeState(aNextState);

            currActiveState = aNextState;

            switchCooldown = true;

        }

        public DetectedType SearchForTargets()
        {
            if (searchTimer < 0.5f || hearingFunctionSettings.HeardSomething)
            {
                Debug.Log("Search Cooldown");
                return DetectedType.None;
            }

            Debug.Log("Searching for targets");

            
             int targetCount =  Physics.OverlapSphereNonAlloc(transform.position, searchFunctionSettings.radius, AIRange, searchFunctionSettings.TargetMask);

            if (AIRange.Length == 0)
            {
                return DetectedType.None;
            }
                

                for (int i = 0; i < targetCount; i++)
                {
                    Debug.Log("Target found");
                    if (!AIRange[i].TryGetComponent<TargetScript>(out TargetScript target))
                    {
                         continue;
                    }
                
                Vector3 directionToTarget = (target.gameObject.transform.position - searchFunctionSettings.Eyes.transform.position).normalized;

                    if (Vector3.Angle(searchFunctionSettings.Eyes.transform.forward, directionToTarget) < searchFunctionSettings.angle / 2)
                    {

                        float distanceToTarget = Vector3.Distance(transform.position, target.gameObject.transform.position);

                        if (!Physics.Raycast(searchFunctionSettings.Eyes.transform.position, directionToTarget, Mathf.Min(distanceToTarget, searchFunctionSettings.radius), searchFunctionSettings.EnvironmentMask))
                        {

                            Debug.Log("Target seen");

                            return CurrentTargetAnalysis(target);


                        }

                    }
                    
                }

            
            return DetectedType.None;

        }

        private DetectedType CurrentTargetAnalysis(TargetScript aTarget)
        {

            if (aTarget.CompareTag("Player"))
            {
                Debug.Log("Player Detected");
                searchFunctionSettings.playerObj = aTarget;

                return DetectedType.Player;
            }

            //Starting case, the first target spotted, will be the target regardless of status
            if (!aTarget.TargetInfo.wasCompleted)
            {
                searchFunctionSettings.CurrTarget = aTarget;

                Debug.Log("New object is set, proceed with interact state");

                return DetectedType.Object;
            }

            return DetectedType.None;
        }



        public void HeardTargetFunction(Vector3 soundLocation) 
        {
            if (searchStateSettings.heardSomething)
            {
                searchStateSettings.noiseLocation = soundLocation;

                searchStateSettings.heardSomething = true;

                SwitchStates(search);

                
            }
        }

    }
}

