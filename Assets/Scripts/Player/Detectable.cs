using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detectable : MonoBehaviour
{
    public GameObject Indicator;
    public bool hideBase = false;
    MeshRenderer mesh;
    private void Start()
    {
        if(mesh == null)
        {
            mesh = GetComponent<MeshRenderer>();
        }
    }
    public void showIndicator()
    {
        if (hideBase)
        {
            if (mesh == null) return;
            mesh.enabled = false;
        }
    }

    public void hideIndicator()
    {
        if (hideBase)
        {
            if (mesh == null) return;
            mesh.enabled = true;
        }
    }
}
