using System;
using UnityEngine;

// manages the rotation of the level 1 setup
public class RotateLevelOne : MonoBehaviour
{
    // the speed of rotation defined as angle per second
    private static readonly float ANGULAR_VELOCITY = 45f;
    // the target angle that we are rotating towards
    private float targetAngle;
    // how much angle have we rotated (during a rotation)
    private float rotatedAngle;
    // the level 1 object
    private Transform level1;

    // are we in initial position or not
    public bool Initial { get; set; }
    // are we in the process of rotating
    public bool Rotating { get; set; }

    // initialize the state of our level rotation
    void Start()
    {
        level1 = GameObject.Find("Level1").transform;
        Rotating = false;
        Initial = true;
    }

    // rotate on demand in a single update cycle
    void Update()
    {
        // check if the collectable object has disappeared
        // and if so, trigger a rotation
        if (Initial)
        {
            var collectable = GameObject.FindWithTag("crystal");
            if (collectable == null) Rotate();
        }

        // rotate the level 1 object
        if (Rotating)
        {
            // find the player
            var player = GameObject.Find("Player");
            // angle to rotate in a single update cycle
            var updateAngle = ANGULAR_VELOCITY * Time.deltaTime;
            rotatedAngle -= updateAngle;
            // stop the player from moving
            if (player != null) player.GetComponent<PlayerController>().KillMovement();
            if (rotatedAngle < targetAngle) rotatedAngle = targetAngle;
            level1.rotation = Quaternion.Euler(0f, rotatedAngle, 0f);
        }

        // flip the rotation-in-progress indicator once rotation is finished
        if (Math.Abs(targetAngle - rotatedAngle) < 0.1f && Rotating)
            Rotating = false;

        // set the active state depending on whether in initial position or not
        ActivateBlocks(Initial);
    }

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
    }

    // selectively activate the blocks depending on whether we are in the initial position or not
    private void ActivateBlocks(bool isInitial)
    {
        if (!isInitial)
        {
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g1"))
                ActivateSingleBlock(i);
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g2"))
                ActivateSingleBlock(i);
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g3"))
                ActivateSingleBlock(i);
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g4"))
                ActivateSingleBlock(i);
        }
    }

    // common logic for activating a single block
    private void ActivateSingleBlock(GameObject obj)
    {
        obj.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
        foreach (var i in obj.transform.GetComponent<Walkable>().possiblePath)
            i.active = true;
    }
}
