using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextZone : MonoBehaviour
{
    public int scene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToLower().Equals("player")) SceneManager.LoadScene(scene);
    }
}
