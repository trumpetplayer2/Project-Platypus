using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;



/// <summary>
/// The type of object that is detected and how to react to it
/// </summary>
public enum DetectedType
{
    None,
    Player,
    Object
}

/// <summary>
/// The number of states that can be the current state
/// </summary>
public enum StateMachineEnum
{
    Initial,
    Idle,
    Patrol,
    Interact,
    Chase,
    Search,
    Observe,
    PlayerDetected
}

/// <summary>
/// What the AI does upon seeing the player
/// </summary>
public enum AIResponse
{
    Chase,
    Observe

}

/// <summary>
/// What the AI does when the player shoots an object within range.
/// </summary>
public enum TriggeredResponse
{
    None,
    Chase
}

///
/// This namespace allows me to organize my variables for each state by their appropriate functionality,
/// making it easier and more designer friendly in the inspector
///
namespace StateMachineInfo
{

    #region StateSettings

    [System.Serializable]
    public class IdleSettings
    {
        /// holds a float that is used with a timer in the state
        public int idleTimeUntil;
    }

   
    [System.Serializable]
    public class PatrolSettings
    {
        /// Holds an array of transforms that the AI will move to
        public Transform[] PatrolDestinations;

        /// Holds the current transform that the AI is moving towards
        [ReadOnly] public Transform CurrPatrolDestination = null;

        /// at what distance does the AI stop at a destination
        public float patrolDistanceToDestination;
    }

    [System.Serializable]
    public class InteractSettings
    {
        /// <summary>
        /// Distance to stop at interactable target
        /// </summary>
        public float distanceBetweenTarget;
    }

    [System.Serializable]
    public class ChaseSettings
    {
        /// <summary>
        /// Adds to chase speed of AI instance
        /// </summary>
        public float chaseSpeedIncrease;

        /// <summary>
        /// The distance for the AI to start losing the player
        /// </summary>
        public float chaseMaxDistance;

        /// <summary>
        /// the distance for the AI to start catching the player
        /// </summary>
        public float chaseMinDistance;

        /// <summary>
        /// How long to wait to change states when losing targets
        /// </summary>
        public float losingTargetTime;

        /// <summary>
        /// How long to wait to change states when player is in range to catch
        /// </summary>
        public float catchTargetTime;

        /// <summary>
        /// the position on the AI that the player will be placed when caught
        /// </summary>
        public GameObject playerGrabbedPosition;

        /// <summary>
        /// the checkpoint position that the AI will take the player towards before letting them go.
        /// </summary>
        public CheckpointTrigger grabbedPlayerLocation;

        /// <summary>
        /// Cooldown that restricts AI ability to catch
        /// </summary>
        public float catchCooldown;
    }

    [System.Serializable]
    public class ObserveSettings
    { 
        /// <summary>
        /// Maximum distance between AI and player before it changes from Observe State
        /// </summary>
        public float maxObserveDistance;
    }

    [System.Serializable]
    public class SearchStateSettings
    { 
        /// <summary>
        /// How long the AI will search for
        /// </summary>
        public float searchStateTime;

        /// <summary>
        /// If the AI hears a "noise", what location will they move towards to investigate
        /// </summary>
        [ReadOnly] public Vector3 noiseLocation;

        /// <summary>
        /// Boolean to denote if the ai is searching location of a noise and to not interfere with this process
        /// </summary>
        [ReadOnly] public bool heardSomething;

        /// <summary>
        /// how long will the AI investigate the location of the noise
        /// </summary>
        public float heardNoiseTime;

        /// <summary>
        /// bool that prevents the AI from repeatedly hearing the same sound
        /// </summary>
        [ReadOnly] public bool alreadyHeardSomething;

        /// <summary>
        /// After investigating noise, will no longer responsd to noises until timer reaches this value
        /// </summary>
        public float hearingCooldown;

        /// <summary>
        /// Timer for hearing cooldown process
        /// </summary>
        [ReadOnly] public float hearingCooldownTime = 0f;
    }

    [System.Serializable]
    public class PlayerDetectedSettings
    {

       [ReadOnly] public bool playerDetectedCooldown = false;

        public float playerDetectedCooldownTime;

        [ReadOnly] public float pDCooldownTimer = 0;
        /// <summary>
        /// Has the AI detected a message from an object hit by gravel from the player?
        /// </summary>
        [ReadOnly] public bool TriggerDetected;

        /// <summary>
        /// The primary AI response to the player, unless a trigger is received
        /// </summary>
        public AIResponse setAIResponse;

        /// <summary>
        /// A secondary enum that determines what response the AI has to the trigger object being shot
        /// </summary>
        public TriggeredResponse setAITriggerResponse;

        public bool itemCheck;

        public string heldObjTypeTrigger;
    }

    [System.Serializable]
    public class SearchFunctionSettings
    {
        /// <summary>
        /// A reference to the transform of the Eyes GameObject, which is where the raycast will be casted from
        /// </summary>
        public Transform Eyes;

        /// <summary>
        /// radius of Overlap Sphere for detection function
        /// </summary>
        public float radius;

        /// <summary>
        /// The specific angle of the overlap sphere where the AI will react to objects within the angle
        /// </summary>
        [Range(0, 360)]
        public float angle;

        /// <summary>
        /// The layers that targets exist on
        /// </summary>
        public LayerMask TargetMask;

        /// <summary>
        /// The layers of the environment that will not be detected or pierced through.
        /// </summary>
        public LayerMask EnvironmentMask;

        /// <summary>
        /// The current target that is being interacted with
        /// </summary>
        [ReadOnly] public TargetScript CurrTarget = null;

        /// <summary>
        /// The player object, if scene
        /// </summary>
        [ReadOnly] public TargetScript playerObj = null;

        /// <summary>
        /// the maximum number of collider that are cycled through by the AI
        /// </summary>
        public int maxColliders;

    }

    #endregion

    public class AIBase : MonoBehaviour
    {
        
        /// <summary>
        /// Shows the current State that the AI is currently processing
        /// </summary>
       [ReadOnly] public StateMachineEnum stateMachineEnum;

        /// <summary>
        /// The Objects for the StateSetting class, referred to in code
        /// </summary>
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

        /// <summary>
        /// The objects for State Objects, referred to in code
        /// </summary>
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

        /// <summary>
        /// reference to component for NavMeshAgent
        /// </summary>
        public NavMeshAgent agent;

        /// <summary>
        /// active state whose functionality is called in the Update function.
        /// </summary>
        public BaseStateClass currActiveState;

        /// <summary>
        /// The amount of time to cooldown before switching states
        /// </summary>
        public float stateSwitchTime;


        /// <summary>
        /// The amount of time to cooldown before searching for potential targets.
        /// </summary>
        public float aIDetectionTime;

     
        [ReadOnly] public float stateSwitchTimer;

        /// <summary>
        /// switch States functionality is on cooldown
        /// </summary>
        bool switchCooldown;

        [ReadOnly] public float searchTimer;

        /// <summary>
        /// Sets the speed of the AI
        /// </summary>
        public float speedVal;

        /// <summary>
        /// array of colliders that are searched by the AI
        /// </summary>
        Collider[] AIRange;

        public Animator aIAnimator;
      
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

            agent.speed = speedVal;

            AIRange = new Collider[searchFunctionSettings.maxColliders];
        }

        void OnEnable()
        {
            currActiveState = initial;
        }

        /// <summary>
        /// calls the functionality of the current state repeatedly
        /// 
        /// runs the timers for switching states, searching, and for hearing
        /// </summary>
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


            if(searchStateSettings.alreadyHeardSomething)
            { 
                 searchStateSettings.heardSomething = false;

                searchStateSettings.hearingCooldownTime += Time.deltaTime;

                if(searchStateSettings.hearingCooldownTime >= searchStateSettings.hearingCooldown)
                {
                    searchStateSettings.hearingCooldownTime = 0;

                    searchStateSettings.alreadyHeardSomething = false;
                }
            }

            if (playerDetectedSettings.playerDetectedCooldown)
            {
                Debug.Log("player Detected Cooldown");
                playerDetectedSettings.pDCooldownTimer += Time.deltaTime;

                if(playerDetectedSettings.pDCooldownTimer >= playerDetectedSettings.playerDetectedCooldownTime)
                {
                    playerDetectedSettings.pDCooldownTimer = 0;

                    playerDetectedSettings.playerDetectedCooldown = false;
                }
            }

        }

        /// <summary>
        /// handles the functionality of switching between states
        /// </summary>
        /// <param name="aNextState"> the state to be switched to </param>

        public void SwitchStates(StateMachineEnum aNextState)
        {
            if (switchCooldown)
            {
                
                return;
            }

            

            BaseStateClass nextStateInput = null;

            stateMachineEnum = aNextState;

            switch (aNextState)
            {
                case StateMachineEnum.Idle:
                    {
                        nextStateInput = idle;
                        break;
                    }
                case StateMachineEnum.Patrol:
                    {
                        nextStateInput = patrol; break;
                        
                    }
                case StateMachineEnum.Interact:
                    {
                        nextStateInput = interact; break;
                    }
                case StateMachineEnum.Chase:
                    {
                        nextStateInput = chase; break;
                    }
                case StateMachineEnum.Search:
                    {
                        nextStateInput = search; break;
                    }
                case StateMachineEnum.Observe:
                    {
                        nextStateInput = observe; break;
                    }
                case StateMachineEnum.PlayerDetected:
                    {
                        nextStateInput = playerDetected; break;
                    }
            }

            if (currActiveState == nextStateInput)
            {
                return;
            }

            currActiveState.ChangeState(nextStateInput);

            currActiveState = nextStateInput;

            switchCooldown = true;

        }

        /// <summary>
        /// the function that searches for the player and interactable targets within a specified angle
        /// </summary>
        /// <returns> returns an enum detailing what item was found</returns>
        public DetectedType SearchForTargets()
        {
            if (searchTimer < 0.5 || searchStateSettings.heardSomething || playerDetectedSettings.playerDetectedCooldown)
            {
                
                
                return DetectedType.None;
            }

           

            
             int targetCount =  Physics.OverlapSphereNonAlloc(transform.position, searchFunctionSettings.radius, AIRange, searchFunctionSettings.TargetMask);

            if (AIRange.Length == 0)
            {
                return DetectedType.None;
            }
                

                for (int i = 0; i < targetCount; i++)
                {
                    
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

                           

                            return CurrentTargetAnalysis(target);


                        }

                    }
                    
                }

            
            return DetectedType.None;

        }

        /// <summary>
        /// Analyzes the found target from the search function and sets up a reference to it in the script
        /// </summary>
        /// <param name="aTarget"> the found object</param>
        /// <returns> returns what type of item was found</returns>
        private DetectedType CurrentTargetAnalysis(TargetScript aTarget)
        {
            

            if (aTarget.CompareTag("Player"))
            {
               
                searchFunctionSettings.playerObj = aTarget;

                return DetectedType.Player;
            }

            //Starting case, the first target spotted, will be the target regardless of status
            if (!aTarget.TargetInfo.wasCompleted)
            {
                searchFunctionSettings.CurrTarget = aTarget;


                return DetectedType.Object;
            }

            return DetectedType.None;
        }


        /// <summary>
        /// function that is called if the AI is within range of a sound
        /// </summary>
        /// <param name="soundLocation"> The Location of the Sound</param>
        public void HeardTargetFunction(Vector3 soundLocation) 
        {
            if (searchStateSettings.alreadyHeardSomething)
            {
                

                return;
            }

            searchStateSettings.noiseLocation = soundLocation;

            searchStateSettings.heardSomething = true;

            SwitchStates(StateMachineEnum.Search);

                
            
        }

        /// <summary>
        ///  Called when the AI is in range of a trigger, and will set the behavior in the player detected script
        /// </summary>
        public void TriggerBehavior()
        {
            playerDetectedSettings.TriggerDetected = true;

            SwitchStates(StateMachineEnum.PlayerDetected);
        }


        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Hit by Gravel");
            if(collision.gameObject.TryGetComponent<Gravel>(out Gravel gravelProjectile)) {
            
                TriggerBehavior();
            }
        }



    }
}

