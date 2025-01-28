using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detectable : MonoBehaviour
{
    public GameObject Indicator;
    public bool hideBase;
    public MeshRenderer mesh;
    private void Start()
    {
        if(mesh == null)
        {
            mesh = GetComponent<MeshRenderer>();
        }
    }
    public void showIndicator()
    {
        Indicator.SetActive(true);
        if (hideBase)
        {
            if (mesh == null) return;
            mesh.enabled = false;
        }
    }

    public void hideIndicator()
    {
        Indicator.SetActive(false);
        if (hideBase)
        {
            if (mesh == null) return;
            mesh.enabled = true;
        }
    }
}
