using StateMachineInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTestScript : MonoBehaviour
{
    

    public float radius;

    float timer;

    public float timerValue;

    bool deployingSphere;

    int heardThis;

    Collider[] soundColliders;

    public LayerMask targetMasks;

    void Awake()
    {
        soundColliders = new Collider[3];

        timer = timerValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0) {

            heardThis = Physics.OverlapSphereNonAlloc(this.transform.position, radius, soundColliders, targetMasks);

            for (int i = 0; i < heardThis; i++)
            {
                if (soundColliders[i].TryGetComponent<AIBase>(out AIBase aI))
                {
                    aI.HeardTargetFunction(this.transform.position);

                }
            }

            timer = timerValue;

        }

       

       
    }

   
}
