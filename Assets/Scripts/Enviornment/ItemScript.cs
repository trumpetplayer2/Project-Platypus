using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class ItemScript : MonoBehaviour
{
    public Transform alignLocation;
    public Transform alignTarget;
    PlayerAbilityManager abilityManager;
    bool isHeld = false;
    public bool holdable = true;
    Rigidbody rb = null;
    UnityAction call;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void grab(Transform target, bool priority = false)
    {
        if (isHeld && !priority) return;
        alignTarget = target;
        transform.SetParent(alignTarget);
        
    }

    public void grab(PlayerAbilityManager abilityManager, bool priority = false)
    {
        if (isHeld && !priority) return;
        Debug.Log("Grab");
        Transform target = abilityManager.grab.holdLocation;
        alignTarget = target;
        transform.SetPositionAndRotation(alignTarget.position, alignTarget.rotation);
        Vector3 scale = transform.lossyScale;
        transform.SetParent(alignTarget);
        transform.localScale = new Vector3(transform.localScale.x/ scale.x, transform.localScale.y/ scale.y, transform.localScale.z/ scale.z);
        abilityManager.grab.heldObject = this;
        this.abilityManager = abilityManager;
        if(rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public void release()
    {
        Vector3 scale = transform.lossyScale;
        alignTarget = null;
        this.transform.parent = null;
        abilityManager.grab.heldObject = null;
        abilityManager = null;
        transform.localScale = scale;
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerAbilityManager>(out PlayerAbilityManager ability))
        {
            if(call != null)
            {
                return;
            }
            call = () => grab(ability);
            ability.grab.grabEvent.AddListener(call);
        }
        //Insert NPC grab check here

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerAbilityManager>(out PlayerAbilityManager ability))
        {
            try
            {
                ability.grab.grabEvent.RemoveListener(call);
                call = null;
            }
            catch { }
        }
        //Insert NPC grab check here
    }
}
