using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool open = false;
    [Header("Open")]
    public Vector3 openPos = Vector3.zero;
    public Vector3 openRotation = Vector3.zero;
    [Header("Closed")]
    public Vector3 closePos = Vector3.zero;
    public Vector3 closeRotation = Vector3.zero;

    public void toggleOpen()
    {
        if(open) { Open(); } else { Close(); }
    }

    public void Open()
    {
        transform.position = openPos;
        transform.rotation = Quaternion.Euler(openRotation);
        open = true;
    }

    public void Close()
    {
        transform.position = closePos;
        transform.rotation = Quaternion.Euler(closeRotation);
        open = false;
    }
}
