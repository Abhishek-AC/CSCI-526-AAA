using UnityEngine;

public class RotationManager_Level3_Key : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //the following code connects cubes after rotation has been made
        Transform cubeToConnect1 = transform.Find("Cube (64)");
        Transform cubeToConnect2 = GameObject.Find("Cube (30)").transform;
        //return if Transform not found
        if (cubeToConnect1 == null || cubeToConnect2 == null)
        {
            Debug.Log("Cube not reachable");
            return;
        }
        //check if the rotation angle is 270 degree, which connects two b   locks
        if (Mathf.Abs(transform.rotation.eulerAngles.z - 0f) < 0.1f)
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
