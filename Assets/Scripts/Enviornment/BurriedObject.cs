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
        Debug.Log(timer);
        timer += Time.deltaTime;
        treasure.transform.position = Vector3.Lerp(startLocation, endLocation.position, (timer)/digTime);
        if(digTime <= timer)
        {
            treasure.transform.parent = null;
            treasure.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
