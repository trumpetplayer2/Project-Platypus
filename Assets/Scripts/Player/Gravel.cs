using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravel : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        if (GameManager.instance.isPaused) return;
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") return;
        Destroy(this.gameObject);
    }
}
