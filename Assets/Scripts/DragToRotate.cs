using System;
using UnityEngine;

// manages the rotation of the level 1 setup
public class DragToRotate : MonoBehaviour
{
    public int rotationSpeed;
    public float snapBlockAngle = 5f;
    // the target angle that we are rotating towards
    private float targetAngle;
    // how much angle have we rotated (during a rotation)
    private float rotatedAngle;
    public GameObject level1;
    // initialize the state of our level rotation
    void Start() => level1 = GameObject.Find("Level1");

    void Update()
    {
        var angle = level1.transform.localEulerAngles.y;
        angle = (angle > 180) ? angle - 360 : angle;
        if (Math.Abs(angle) < snapBlockAngle)
        {
            Vector3 rotationAngles;
            rotationAngles.y = 0f;
            rotationAngles.x = 0f;
            rotationAngles.z = 0f;
            level1.transform.localEulerAngles = rotationAngles;
        }
    }
    
    // Start is called before the first frame update
    void OnMouseDrag()
    {
        var collectable = GameObject.Find("Collectable");
        var player = GameObject.Find("Player");
        // only allow rotation if the collectable is not present
        if (collectable == null && player != null)
        {
            GameObject.Find("rotate-sfx").GetComponent<AudioSource>().Play();  //SFX
            var yaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.Rotate(Vector3.down, -yaxisRotation);
            level1.transform.Rotate(Vector3.down, -yaxisRotation);
            if (Math.Abs(level1.transform.localEulerAngles.y) <= 0.1f)
                ActivateBlocks(true);
            else
                ActivateBlocks(false);
        }
    }
    
    // selectively activate the blocks depending on whether we are in the initial position or not
    private void ActivateBlocks(bool activeFlag)
    {
        foreach (var i in GameObject.FindGameObjectsWithTag("l1g1"))
            ActivateSingleBlock(i, activeFlag);
        foreach (var i in GameObject.FindGameObjectsWithTag("l1g2"))
            ActivateSingleBlock(i, activeFlag);
        foreach (var i in GameObject.FindGameObjectsWithTag("l1g3"))
            ActivateSingleBlock(i, activeFlag);
        foreach (var i in GameObject.FindGameObjectsWithTag("l1g4"))
            ActivateSingleBlock(i, activeFlag);
    }
    
    // common logic for activating a single block
    private void ActivateSingleBlock(GameObject obj, bool activeFlag)
    {
        obj.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
        foreach (var i in obj.transform.GetComponent<Walkable>().possiblePath)
            i.active = activeFlag;
    }
}