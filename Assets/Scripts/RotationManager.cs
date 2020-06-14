using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //the following code connects cubes after rotation has been made
        Transform cubeToConnect1 = transform.Find("Cube (y4)");
        Transform cubeToConnect2 = GameObject.Find("Cube (x-5y8z1)").transform;
        //return if Transform not found
        if(cubeToConnect1 == null || cubeToConnect2 == null)
        {
            
            return;
        }
        //check if the rotation angle is 270 degree, which connects two blocks
        if (Mathf.Abs(transform.rotation.eulerAngles.z - 270f) < 0.1f)
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[0].active = true;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = true;
        }
        else
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[0].active = false;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = false;
        }
    }
}
