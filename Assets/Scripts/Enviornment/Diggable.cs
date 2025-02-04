using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;

public abstract class Diggable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerAbilityManager>(out PlayerAbilityManager ability))
        {
            ability.dig.dig.AddListener(() => dig());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerAbilityManager>(out PlayerAbilityManager ability))
        {
            try
            {
                ability.dig.dig.RemoveListener(() => dig());
            }
            catch { }
        }
    }

    public abstract void dig();
}
