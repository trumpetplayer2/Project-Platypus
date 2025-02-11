using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerMovement>(out var movement)) return;
        movement.swimming = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<PlayerMovement>(out var movement)) return;
        movement.swimming = false;
    }
}
