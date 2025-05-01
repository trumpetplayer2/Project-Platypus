using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurriedObject : Diggable
{
    public GameObject treasure;
    Vector3 startLocation;
    public Transform endLocation;
    bool digging = false;
    float timer = 0;
    public float digTime = 1f;
    public int itemLayer = 11;
    public GameObject digParticlePrefab;
    GameObject digParticle;
    public override void dig()
    {
        if (triggered) return;
        base.dig();
        timer = 0;
        digging = true;
        treasure.layer = 0;
        //Instantiate dig particles
        if(digParticlePrefab != null)
        {
            digParticle = Instantiate(digParticlePrefab, transform);
        }
        if (treasure.TryGetComponent<ItemScript>(out ItemScript i))
        {
            i.holdable = false;
        }
    }
    private void Start()
    {
        startLocation = transform.position;
    }
    private void Update()
    {
        if(!digging) { return; }
        timer += Time.deltaTime;
        treasure.transform.position = Vector3.Lerp(startLocation, endLocation.position, (timer)/digTime);
        if(digTime <= timer)
        {
            treasure.transform.parent = null;
            treasure.gameObject.SetActive(true);
            if (treasure.TryGetComponent<ItemScript>(out ItemScript item))
            {
                item.holdable = true;
                item.setSpawn(item.transform);
            }
            if(treasure.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.useGravity = true;
                rigidbody.velocity = Vector3.zero;
            }
            treasure.layer = 11;
            treasure.gameObject.SetActive(true);
            if(digParticle != null)
            {
                Destroy(digParticle.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
