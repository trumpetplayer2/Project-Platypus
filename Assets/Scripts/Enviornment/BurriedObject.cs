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
        digging = true;
    }
    private void Start()
    {
        startLocation = transform.position;
    }
    private void Update()
    {
        if(!digging) { return; }
        timer += Time.deltaTime;
        treasure.transform.position = Vector3.Lerp(startLocation, endLocation.position, timer/digTime);
        if(digTime >= timer)
        {
            treasure.transform.parent = null;
            Destroy(this.gameObject);
        }
    }
}
