using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GravelHit : MonoBehaviour
{
    public UnityEvent hitEvent = new UnityEvent();
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Gravel>(out Gravel gravel))
        {
            hitEvent.Invoke();
        }
    }
}
