using System;
using System.Collections;
using System.Collections.Generic;
using tp2;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public enum ItemType
{
    Other, Ball, Bell, Key, None
}
[Serializable]
public class ItemScript : MonoBehaviour
{
    public Transform alignLocation;
    public Transform alignTarget;
    public ItemType type = ItemType.Other;
    public float minHeight = -100f;
    public Vector3 respawnPoint;
    PlayerAbilityManager abilityManager;
    public bool isHeld = false;
    public bool holdable = true;
    Rigidbody rb = null;
    UnityAction call;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        setSpawn(transform);
    }

    public void grab(Transform target, bool priority = false)
    {
        if (isHeld && !priority) return;
        alignTarget = target;
        transform.parent = alignTarget;
        transform.position = alignTarget.position;
        
    }

    private void FixedUpdate()
    {
        if(transform.position.y <= minHeight)
        {
            respawn(transform);
        }
    }

    public void setSpawn(Transform t)
    {
        ////Go to highest nonparent
        //if (t.parent != null)
        //{
        //    //If parent is player, dont respawn
        //    if (t.parent.tag.ToLower().Equals("player")) return;
        //    setSpawn(t.parent);
        //    return;
        //}
        respawnPoint = t.position;
    }

    public void respawn()
    {
        respawn(transform);
    }

    void respawn(Transform t)
    {   
        ////Go to highest nonparent
        //if (t.parent != null)
        //{
        //    if (t.parent.tag.ToLower().Equals("player")) return;
        //    respawn(t.parent);
        //    return;
        //}
        //Send highest nonparent to spawnpoint
        t.position = respawnPoint;
        if(rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    public void grab(PlayerAbilityManager abilityManager, bool priority = false)
    {
        if (isHeld && !priority) return;
        Transform target = abilityManager.grab.holdLocation;
        alignTarget = target;
        transform.SetPositionAndRotation(alignTarget.position, alignTarget.rotation);
        transform.SetParent(alignTarget);
        //transform.localScale = new Vector3(transform.localScale.x/ scale.x, transform.localScale.y/ scale.y, transform.localScale.z/ scale.z);
        abilityManager.grab.heldObject = this;
        this.abilityManager = abilityManager;
        if(rb != null)
        {
            rb.isKinematic = true;
        }
        isHeld = true;
    }

    public void release()
    {
        alignTarget = null;
        this.transform.parent = null;
        abilityManager.grab.heldObject = null;
        abilityManager = null;
        isHeld = false;
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
