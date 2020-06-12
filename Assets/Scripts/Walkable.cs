using System.Collections;
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
    //get walkPoint of current cube
    public Vector3 GetWalkPoint()
    {
        return transform.position + transform.up * (1 - walkPointOffset);
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
