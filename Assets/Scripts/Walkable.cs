using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    //this value is only defined for cube that is on the edge
    public Vector3 edgeValue;
    //list of possible blocks this block can go to
    public List<GamePath> possiblePath = new List<GamePath>();
    public Transform previousBlock;
    public float walkPointOffset = 1f;
    public bool canWalkOnThisBlock = true;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    //get walkPoint of current cube
    public Vector3 GetWalkPoint()
    {
        if (this.canWalkOnThisBlock == false)
        {
            return new Vector3(0, -0.5f, 0);
        }

        // this part of the code only applies to level 2
        // before we find a better way to differentiate it
        // let's test the presence of an object specific to level 2
        GameObject rotateObject = GameObject.Find("Rotate");
        if (rotateObject != null)
        {
            foreach (Transform t in rotateObject.transform)
            {
                t.gameObject.GetComponent<Walkable>().canWalkOnThisBlock = true;
            }

            if (transform.tag == "rotatableCube")
            {
                /* 
                for rotatable cubes handling two primary cases,
                1. If there is cube object above the clickedCube then 
                   the capsule is not allowed to go there.
                2. The Walkable points are drawn as per capsule's alignment
                */
                GameObject capsuleObject = GameObject.Find("Player");
                if (Physics.Raycast(transform.position, capsuleObject.transform.up, 10f))
                {
                    canWalkOnThisBlock = false;
                    return new Vector3(0, -0.5f, 0);
                }
                return transform.position + capsuleObject.transform.up * (1 - walkPointOffset);
            }
        }

        return transform.position + transform.up * (1 - walkPointOffset) + new Vector3(offsetX, offsetY, offsetZ);
    }
    //draw gismos sphere to show the walk path
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetWalkPoint(), 0.1f);
    }
}
[System.Serializable]
public class GamePath
{
    public Transform target;
    public bool active;
}
