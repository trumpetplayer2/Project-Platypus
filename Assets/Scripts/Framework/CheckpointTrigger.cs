using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Vector3 checkpointPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.tag.ToLower().Equals("player")) return;
        PauseScript.instance.checkpoint = checkpointPosition;
        if (Vector3.Distance(PauseScript.instance.checkpoint, checkpointPosition) > 0.1)
        {
            //New checkpoint, Save
            GameManager.instance.save();
        }
    }
}
