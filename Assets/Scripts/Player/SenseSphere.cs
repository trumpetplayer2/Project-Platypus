using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SenseSphere : MonoBehaviour
{
    public List<Detectable> detected = new List<Detectable>();
    public RawImage sense;
    private void Start()
    {
        foreach(MeshRenderer renderer in FindObjectsOfType<MeshRenderer>())
        {
            if(renderer.gameObject.layer == 6)
            {
                renderer.enabled = false;
                if (!renderer.gameObject.TryGetComponent<Detectable>(out _))
                {
                    Detectable t = renderer.gameObject.AddComponent<Detectable>();
                    t.mesh = renderer;
                    t.Indicator = renderer.gameObject;
                }
                if(!renderer.gameObject.TryGetComponent<Collider>(out _))
                {
                    Collider c = renderer.gameObject.AddComponent<BoxCollider>();
                    c.isTrigger = true;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //If thing that entered isnt detectable, ignore it
        if (other.TryGetComponent<Detectable>(out Detectable temp)) { detected.Add(temp); return; } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Detectable>(out Detectable temp)) { detected.Remove(temp); temp.hideIndicator(); }
    }

    public void showDetect()
    {
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
