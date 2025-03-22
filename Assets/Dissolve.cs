using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private void Start()
    {
        transform.DetachChildren();
        Destroy(this.gameObject);
    }
}
