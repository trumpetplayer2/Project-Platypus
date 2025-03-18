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
    public override void dig()
    {
        base.dig();
        timer = 0;
        digging = true;
        treasure.layer = 0;
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
            if(treasure.layer == 6)
            {
                treasure.layer = 0;
            }
            treasure.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
