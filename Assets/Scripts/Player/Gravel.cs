using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravel : MonoBehaviour
{
    public float speed;
    public LayerMask ignoreLayer;
    private void Update()
    {
        if (GameManager.instance.isPaused) return;
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") return;
        if (other.gameObject.layer == 6) return;
        if(other.isTrigger) return;
        if (((ignoreLayer & (1 << other.gameObject.layer)) != 0))
        {
            return;
        }
        Destroy(this.gameObject);
    }
}
