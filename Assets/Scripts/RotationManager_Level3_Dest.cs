using UnityEngine;

public class RotationManager_Level3_Dest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //the following code connects cubes after rotation has been made
        Transform cubeToConnect1 = transform.Find("Cube (71)");
        Transform cubeToConnect2 = GameObject.Find("Cube (50)").transform;
        Transform cubeToConnect3 = GameObject.Find("Cube (48)").transform;
        //return if Transform not found
        if (cubeToConnect1 == null || cubeToConnect2 == null)
        {
            Debug.Log("Cube not reachable");
            return;
        }
        //check if the rotation angle is 270 degree, which connects two blocks
        if (Mathf.Abs(transform.rotation.eulerAngles.x - 90f) < 0.1f)
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[1].active = true;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = true;
            cubeToConnect3.GetComponent<Walkable>().possiblePath[2].active = false;
            cubeToConnect1.GetComponent<Walkable>().possiblePath[2].active = false;
            cubeToConnect1.GetComponent<Walkable>().canWalkOnThisBlock = true;
        }
        else
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[1].active = false;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = false;
            cubeToConnect1.GetComponent<Walkable>().canWalkOnThisBlock = false;
        }
    }
}
