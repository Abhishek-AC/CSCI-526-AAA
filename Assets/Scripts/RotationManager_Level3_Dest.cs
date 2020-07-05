using UnityEngine;

public class RotationManager_Level3_Dest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //the following code connects cubes after rotation has been made
        Transform cubeToConnect1 = transform.Find("Cube (54)");
        Transform cubeToConnect2 = GameObject.Find("Stairs").transform;
        //return if Transform not found
        if (cubeToConnect1 == null || cubeToConnect2 == null)
        {
            Debug.Log("Cube not reachable");
            return;
        }
        //check if the rotation angle is 270 degree, which connects two blocks
        if (Mathf.Abs(transform.rotation.eulerAngles.x - 90f) < 0.1f)
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[0].active = true;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = true;
            cubeToConnect2.GetComponent<Walkable>().canWalkOnThisBlock = true;
        }
        else
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[0].active = false;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = false;
            cubeToConnect2.GetComponent<Walkable>().canWalkOnThisBlock = false;
        }
    }
}
