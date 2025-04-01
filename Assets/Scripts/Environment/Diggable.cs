using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;
using UnityEngine.Events;

public abstract class Diggable : MonoBehaviour
{
    UnityAction call;
    protected bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerAbilityManager>(out PlayerAbilityManager ability))
        {
            if(call != null) { return; }
            call = () => dig();
            ability.dig.dig.AddListener(call);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerAbilityManager>(out PlayerAbilityManager ability))
        {
            try
            {
                ability.dig.dig.RemoveListener(call);
                call = null;
            }
            catch { }
        }
    }

    public virtual void dig() {
        PlayerAbilityManager.instance.dig.triggered = true;
        triggered = true;
    }
}
