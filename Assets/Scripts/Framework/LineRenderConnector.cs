using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRenderConnector : MonoBehaviour
{
    LineRenderer lineRenderer;
    public GameObject[] connections;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.positionCount = connections.Length;
        for(int i = 0; i < connections.Length; i++)
        {
            lineRenderer.SetPosition(i, connections[i].transform.position);
        }
    }
}
