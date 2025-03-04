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
using UnityEditor.Searcher;
using UnityEngine.UIElements;
using System.ComponentModel;

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
    }

    [System.Serializable]
    public class InteractSettings
    {
        
        [ReadOnly] public TargetScript CurrTarget = null;

        
        [ReadOnly] public TargetScript playerObj = null;
    }

    [System.Serializable]
    public class ChaseSettings
    {
        public float chaseSpeedVal;

        public float chaseMaxDistance;

        public float chaseMinDistance;

        public float losingTargetVal;

        public float catchTimerVal;
    }

    [System.Serializable]
    public class ObserveSettings
    {
        public float rotateTimerVal;

        public float maxDistanceVal;
    }

    [System.Serializable]
    public class SearchStateSettings
    { 
        public float searchStateVal;

        public enum SearchMethod
        {
            SearchInPlace,
            SearchInRandomPoint
        }

        public SearchMethod searchMethod;

    }

    [System.Serializable]
    public class PlayerDetectedSettings
    {

        [ReadOnly] public bool playerFound;

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

    }

    public class AIBase : MonoBehaviour
    {
        public IdleSettings idleSettings;

        public PatrolSettings patrolSettings;

        public InteractSettings interactSettings;

        public ChaseSettings chaseSettings;

        public SearchStateSettings searchStateSettings;

        public PlayerDetectedSettings playerDetectedSettings;

        public SearchFunctionSettings searchFunctionSettings;

        public ObserveSettings observeSettings;

        public InitialState initial = null;

        public IdleState idle = null;

        public PatrolState patrol = null;

        public InteractState interact = null;

        public ChaseState chase = null;

        public SearchState search = null;

        public ObserveState observe = null;

        [ReadOnly] public PlayerDetectedState playerDetected = null;

        public NavMeshAgent agent;

        public float distanceBetweenTarget;


        public BaseStateClass currActiveState;

        public float stateSwitchTimerVal;

        public float searchTimerVal;

        [ReadOnly] public float stateSwitchTimer;

        [ReadOnly] public float searchTimer;

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

            Speed = speedVal;

           
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
                    stateSwitchTimer = stateSwitchTimerVal;
                    switchCooldown = false;
                }
                        
            }

            searchTimer -= Time.deltaTime;

            if (searchTimer <= 0)
                searchTimer = searchTimerVal;

        }

        public void SwitchStates(BaseStateClass aCurrActiveState, BaseStateClass aNextState)
        {
            if (switchCooldown)
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

            switchCooldown = true;

        }

        public bool SearchForTargets()
        {
            if (searchTimer > 1)
            {
                Debug.Log("Search Cooldown");
                return false;
            }

            Debug.Log("Searching for targets");

            Collider[] AIRange = Physics.OverlapSphere(transform.position, searchFunctionSettings.radius, searchFunctionSettings.TargetMask);

            if (AIRange.Length != 0)
            {
                Debug.Log("Target found");

                for (int i = 0; i < AIRange.Length; i++)
                {

                    if (!AIRange[i].TryGetComponent<TargetScript>(out TargetScript target))
                    {
                        continue;
                    }

                    Vector3 directionToTarget = (target.gameObject.transform.position - searchFunctionSettings.Eyes.transform.position).normalized;

                    if (Vector3.Angle(searchFunctionSettings.Eyes.transform.forward, directionToTarget) < searchFunctionSettings.angle / 2)
                    {

                        float distanceToTarget = Vector3.Distance(transform.position, target.gameObject.transform.position);

                        if (!Physics.Raycast(searchFunctionSettings.Eyes.transform.position, directionToTarget, distanceToTarget, searchFunctionSettings.EnvironmentMask))
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
                interactSettings.playerObj = aTarget;

                playerDetectedSettings.playerFound = true;
                return false;
            }

            //Starting case, the first target spotted, will be the target regardless of status
            if (!aTarget.TargetInfo.wasCompleted)
            {
                interactSettings.CurrTarget = aTarget;

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
            if (interactSettings.playerObj != null)
            {
                return interactSettings.playerObj;
            }
            else
            {
                return interactSettings.CurrTarget;
            }

        }




    }
}

