using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3[] positionList;
    public float speed = 1f;
    public float variance = 0.01f;
    public float waitOnTop = 1f;
    float timer = 0f;
    float timeToNext = 0f;
    float freezeTime = 0f;
    int position = 0;
    int lastPos = 0;
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
    private void Start()
    {
        if (timeToNext == 0f) timeToNext = 0.01f;
        timeToNext = Vector2.Distance(transform.position, positionList[position])/speed;
    }
    void Update()
    {
        if(GameManager.instance.isPaused) return;
        if (freezeTime > 0f)
        {
            freezeTime -= Time.deltaTime;
            return;
        }
        if(!moving) return;
        timer += Time.deltaTime;
        transform.position = Vector3.Lerp(positionList[lastPos], positionList[position], timer/timeToNext);
        if (Vector3.Distance(transform.position, positionList[position]) < variance) getNextPlatform();
    }

    void getNextPlatform()
    {
        timer = 0f;
        lastPos = position;
        position = invert ? position + 1 : position - 1;
        if(position < 0)
        {
            position = 1;
            invert = false;
            freezeTime = waitOnTop;
        }
        if(position > positionList.Length)
        {
            position = positionList.Length;
            invert = true;
            freezeTime = waitOnTop;
        }
        timeToNext = Vector2.Distance(positionList[lastPos], positionList[position]) / speed;
    }

    public void toggleMoving()
    {
        moving = !moving;
    }

    public void moveOn()
    {
        moving = true;
    }
}
