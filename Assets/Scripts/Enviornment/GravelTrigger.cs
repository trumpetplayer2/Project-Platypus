using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;

public class GravelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerAbilityManager>(out PlayerAbilityManager ability)) ability.canCollectGravel = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerAbilityManager>(out PlayerAbilityManager ability)) ability.canCollectGravel = false;
    }
}
