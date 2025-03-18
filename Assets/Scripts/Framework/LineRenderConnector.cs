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
        GameObject[] temp2 = connections;
        for(int i = 0; i < connections.Length; i++)
        {
            if (connections[i] == null) {
                GameObject[] temp = new GameObject[temp2.Length - 1];
                int k = 0;
                for(int j = 0; j < temp2.Length; j++)
                {
                    if (k == i) continue;
                    temp[k] = temp2[j];
                    k++;
                }
                temp2 = temp;
                continue;
            }
            lineRenderer.SetPosition(i, connections[i].transform.position);
        }
        if(temp2.Length != connections.Length)
        {
            connections = temp2;
        }
    }
}
