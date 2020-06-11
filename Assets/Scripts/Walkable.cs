using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    public List<GamePath> possiblePath = new List<GamePath>();
    public Transform previousBlock;
    public float walkPointOffset = 0.5f;
    public Vector3 GetWalkPoint()
    {
        return transform.position + transform.up * (1 - walkPointOffset);
    }
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
