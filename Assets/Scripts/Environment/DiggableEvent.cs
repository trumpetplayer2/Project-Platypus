using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiggableEvent : Diggable
{
    public UnityEvent onDig = new UnityEvent();
    public float digTime = 1f;
    public override void dig()
    {
        base.dig();
        Invoke("removeOnDig", digTime);
    }

    public void removeOnDig()
    {
        onDig.Invoke();
        Destroy(this.gameObject);
    }
}
