using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SCTriggerResponse : ScriptableObject
{
  
    public GameObject[] triggerObjects = new GameObject[10];

    public bool playerShouldntShoot;

    public bool playerShouldntBeThere;

}
