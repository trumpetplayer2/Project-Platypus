using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SenseSphere : MonoBehaviour
{
    public List<Detectable> detected = new List<Detectable>();
    public RawImage sense;
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
        Debug.Log("Showing");
        foreach (Detectable detectable in detected)
        {
            detectable.showIndicator();
        }
        sense.enabled = true;
    }

    public void hideDetect()
    {
        foreach (Detectable detectable in detected)
        {
            detectable.hideIndicator();
        }
        sense.enabled = false;
    }
}
