using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Transform child = transform.Find("Cube (y4)");
        Transform cubeToActivate = GameObject.Find("Cube (x-5y8z1)").transform;
        if (Mathf.Abs(transform.rotation.eulerAngles.z - 270f) < 0.1f)
        {
            Debug.Log("Activated");
            if(child != null)
            {
                Debug.Log("children found");
                child.GetComponent<Walkable>().possiblePath[0].active = true;
            }
            cubeToActivate.GetComponent<Walkable>().possiblePath[1].active = true;
        }
        else
        {
            child.GetComponent<Walkable>().possiblePath[0].active = false;
            cubeToActivate.GetComponent<Walkable>().possiblePath[1].active = false;
        }
    }
}
