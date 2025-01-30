using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class AIBase : MonoBehaviour
{



    #region StateMachine Information

    //States for all 
    /// <summary>
    /// Idle: The Transition state between states
    /// 
    /// Patrol: Moving to different points of interest
    /// 
    /// Chasing: Is given a Target to chase, such as the player
    /// 
    /// TargetInteract: The specific event that occurs when the AI reaches the target, such as taking an object from the player.
    /// </summary>
    /// 
    enum States
    {
        Idle,
        Patrol,
        DetectedPlayer,
        Chasing,
        TargetInteract,

    }

    //AI agent's current State
    States CurrState;

    private float Timer = 0;

    public float IdleUntilTime;

    #endregion

    #region AI Variables

    public float AIWalkSpeed;

    public float AIRunMultiplier;

    public float AIVisionRange;

    public LayerMask BlocksVision;

    public Transform Eyes;

    public float TimeUntilIdle;

    private GameObject CurrTarget;

    //List of in game objects that the AI will interact with 
    public List<GameObject> AITargets = new List<GameObject>();

    //public

    #endregion

    #region NavMesh Variables

    NavMeshAgent _agent;



    #endregion

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Determines functionality to perform based on state.
        switch(CurrState)
        {
            case States.Idle:
                {
                    Timer += Time.deltaTime;
                    Idle();
                    break;
                }
            case States.Patrol: 
                {
                    Debug.Log("AI is Patrolling");
                    Patrol();
                    
                    break;
                }
            case States.DetectedPlayer:
                {
                    Debug.Log("Player is Detected");
                    break;
                }
            case States.Chasing:
                {
                    Debug.Log("AI is Chasing Target");
                    Chasing();
                    break;
                }
             case States.TargetInteract:
                {
                    Debug.Log("AI is busy, interacting with object");
                    TargetInteracting();
                    break;
                }
        }
    }



  

    private bool SearchingForPlayer()
    {
        Debug.Log("Detecting Target Function");

        //if(Physics.Raycast(Eyes.position, (CurrTarget)))
        return true;
    }

    ///
    private void TargetInteracting()
    {
        if (SearchingForPlayer())
        {

        }



        Debug.Log("Interaction Function");
    }

    private void Chasing()
    {

        //_agent.destination = 
        Debug.Log("Chasing Function");
    }

    private void Patrol()
    {
        Debug.Log("Patrol Function");

        if (SearchingForPlayer())
        {
            
        }

        int RandomTask = UnityEngine.Random.Range(0, AITargets.Count);

        CurrTarget = AITargets[RandomTask];
       
        _agent.destination = CurrTarget.transform.position;


        if (Vector3.Distance(CurrTarget.transform.position, _agent.gameObject.transform.position) <= 2)
        {

            CurrState = States.TargetInteract;

        }

        

    }



    private void Idle()
    {
        Debug.Log("Idle Function");

        StartCoroutine(ChangeState());
    }

    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(3);

        CurrState = States.Patrol;
    }


    // Start is called before the first frame update
    void Start()
    {
        CurrState = States.Idle;
    }

   
}
