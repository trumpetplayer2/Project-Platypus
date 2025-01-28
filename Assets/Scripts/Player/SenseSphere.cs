using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SenseSphere : MonoBehaviour
{
    public List<Detectable> detected { get; }
    private void OnTriggerEnter(Collider other)
    {
        //If thing that entered isnt detectable, ignore it
        if (other.TryGetComponent<Detectable>(out Detectable temp)) detected.Add(temp);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Detectable>(out Detectable temp)) detected.Remove(temp);
    }

    public void showDetect()
    {
        foreach(Detectable detectable in detected)
        {
            detectable.showIndicator();
        }
    }

    public void hideDetect()
    {
        foreach(Detectable detectable in detected)
        {
            detectable.hideIndicator();
        }
    }
}
