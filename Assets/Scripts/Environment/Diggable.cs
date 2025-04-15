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
            ability.dig.DigObjects.Add(this.gameObject);

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
                ability.dig.DigObjects.Remove(this.gameObject);
            }
            catch { }
        }
    }

    public virtual void dig() {
        PlayerAbilityManager.instance.dig.triggered = true;
        triggered = true;
    }

    private void OnDestroy()
    {
        PlayerAbilityManager ability = PlayerAbilityManager.instance;
        try
        {
            ability.dig.dig.RemoveListener(call);
            call = null;
        }catch { }
        ability.dig.DigObjects.Remove(this.gameObject);
    }
}
