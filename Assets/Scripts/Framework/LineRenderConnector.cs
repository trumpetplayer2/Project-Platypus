using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRenderConnector : MonoBehaviour
{
    LineRenderer lineRenderer;
    public GameObject[] connections;
    public bool canSnap = false;
    public float snapDistance = 5f;
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
        if (!canSnap) return;
        
        for(int i = 0; i < connections.Length - 1; i++)
        {
            if(Vector3.Distance(connections[i].transform.position, connections[i+1].transform.position) > snapDistance)
            {
                if (connections[i].TryGetComponent<ItemScript>(out ItemScript item))
                {
                    
                    if (item.isHeld)
                    {
                        Debug.Log("Pos" + i);
                        item.release();
                        continue;
                    }
                }
                if (connections[i+1].TryGetComponent<ItemScript>(out item))
                {
                    if (item.isHeld)
                    {
                        item.release();
                        continue;
                    }
                }
            }
        }
    }
}
