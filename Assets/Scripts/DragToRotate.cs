using System;
using UnityEngine;
// manages the rotation of the level 1 setup
public class DragToRotate : MonoBehaviour
{
    public int rotationSpeed;
    public float snapBlockAngle = 5f;
    // the speed of rotation defined as angle per second
    //private static readonly float ANGULAR_VELOCITY = 45f;
    // the target angle that we are rotating towards
    private float targetAngle;
    // how much angle have we rotated (during a rotation)
    private float rotatedAngle;
    // the level 1 object
    // public Transform level1;
    // are we in initial position or not
    public bool Initial { get; set; }
    // are we in the process of rotating
    public bool Rotating { get; set; }
    public GameObject Level1;
    // initialize the state of our level rotation
    void Start()
    {
        // level1 = GameObject.Find(“Level1”).transform;
        //GameObject shaft = GameObject.Find(“Shaft_with_spokes”);
        //shaft.SetActive(false);
        Rotating = false;
        Initial = true;
    }
    void Update()
    {
        var collectable = GameObject.Find("Collectable");
        var player = GameObject.Find("Player");
        // if collectable is null, it could also be that the game is paused
        // but if player is not null, then it means the collectable is gone

        var angle = Level1.transform.localEulerAngles.y;
        angle = (angle > 180) ? angle - 360 : angle;
        Debug.Log(angle);
        if (Math.Abs(angle) < snapBlockAngle)
        {
            // set the active state depending on whether in initial position or not
            // level1.rotation = Quaternion.Euler(0f, rotatedAngle, 0f);
            Debug.Log("level 1 transform euler angles less than 10f");
            Vector3 rotationAngles;
            rotationAngles.y = 0f;
            rotationAngles.x = 0f;
            rotationAngles.z = 0f;
            Level1.transform.localEulerAngles = rotationAngles;
            //ActivateBlocks(true);
        }
    }
    // Start is called before the first frame update
    void OnMouseDrag()
    {
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.Rotate(Vector3.down, -YaxisRotation);
        Level1.transform.Rotate(Vector3.down, -YaxisRotation);
        /*
        // rotate the level 1 object
        if (Rotating)
        {
            // find the player
            var player = GameObject.Find(“Player”);
            // angle to rotate in a single update cycle
            var updateAngle = ANGULAR_VELOCITY * Time.deltaTime;
            rotatedAngle -= updateAngle;
            // stop the player from moving
            if (player != null) player.GetComponent<PlayerController>().KillMovement();
            if (rotatedAngle < targetAngle) rotatedAngle = targetAngle;
           // level1.rotation = Quaternion.Euler(0f, rotatedAngle, 0f);
        }
        // flip the rotation-in-progress indicator once rotation is finished
        if (Math.Abs(targetAngle - rotatedAngle) < 0.1f && Rotating)
            Rotating = false;
        */
        if (Math.Abs(Level1.transform.localEulerAngles.y) == 0f)
        {
            // set the active state depending on whether in initial position or not
            // level1.rotation = Quaternion.Euler(0f, rotatedAngle, 0f);
            ActivateBlocks(true);
        }
        else
        {
            ActivateBlocks(false);
        }
    }
    // selectively activate the blocks depending on whether we are in the initial position or not
    private void ActivateBlocks(bool activeFlag)
    {
        //if (!isInitial)
        //{
        foreach (var i in GameObject.FindGameObjectsWithTag("l1g1"))
            ActivateSingleBlock(i, activeFlag);
        foreach (var i in GameObject.FindGameObjectsWithTag("l1g2"))

            ActivateSingleBlock(i, activeFlag);
        foreach (var i in GameObject.FindGameObjectsWithTag("l1g3"))
            ActivateSingleBlock(i, activeFlag);
        foreach (var i in GameObject.FindGameObjectsWithTag("l1g4"))
            ActivateSingleBlock(i, activeFlag);
        //}
    }
    // common logic for activating a single block
    private void ActivateSingleBlock(GameObject obj, bool activeFlag)
    {
        obj.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
        foreach (var i in obj.transform.GetComponent<Walkable>().possiblePath)
            i.active = activeFlag;
    }
    /*
    // set up a rotation
    private void Rotate()
    {
        // initialized the rotated angle variable with current y rotation
        rotatedAngle = level1.eulerAngles.y;
        // set the target angle to zero
        targetAngle = 0f;
        // flip the position state
        Initial = false;
        // start a rotation
        Rotating = true;
    }*/
}