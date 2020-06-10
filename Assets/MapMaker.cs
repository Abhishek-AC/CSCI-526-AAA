using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mape : MonoBehaviour
{
    Vector3[] slopeVertices = new Vector3[]
    {
        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f),

        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, 0.5f, -0.5f),

        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, 0.5f),

        new Vector3(-0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, 0.5f, -0.5f),
        new Vector3(0.5f, 0.5f, 0.5f),
        new Vector3(-0.5f, -0.5f, 0.5f),

        new Vector3(0.5f, -0.5f, -0.5f),
        new Vector3(0.5f, -0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, 0.5f),
        new Vector3(0.5f, 0.5f, -0.5f),


    };

    int[] slopeTriangles = new int[]{
        0, 1, 2,
        0, 2, 3,
        4, 6, 5,
        8, 7, 9,
        10, 12, 11,
        10, 13, 12,
        14, 17, 15,
        15, 17, 16,
    };

    GameObject makeSlope()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Load(int stage_id)
    {
        switch(stage_id)
        {
            case 1:
                MakeMap(new Map01());
                break;
        }
    }
}
