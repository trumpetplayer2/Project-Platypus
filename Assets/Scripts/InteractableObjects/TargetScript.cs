using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public InteractableTarget TargetInfo;


    public InteractableTarget ReturnTargetInfo()
    {
        return TargetInfo;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetInfo.wasCompleted)
        {

        }
    }
}
