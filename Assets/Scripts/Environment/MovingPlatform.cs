using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public Vector3[] positionList;
    float timer = 0f;
    float timeToNext = 0f;
    int position = 0;
    bool moving = false;
    bool invert = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null) return;
        other.transform.parent = this.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.parent != this.transform) return;
        other.transform.parent = this.transform;
    }

    void Update()
    {
        
    }

    void getNextPlatform()
    {
        position = invert ? position++ : position--;
        if(position < 0)
        {
            position = 0;
            invert = false;
            return;
        }
        if(position > positionList.Length)
        {
            position = positionList.Length;
            invert = true;
            return;
        }
    }
}
