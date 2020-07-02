using System;
using UnityEngine;

public class RotateLevelOne : MonoBehaviour
{
    // the speed of rotation defined as angle per second
    private static readonly float ANGULAR_VELOCITY = 45f;

    // are we in the process of rotating
    private bool rotating;
    // are we in initial position or not
    private bool initial;
    // the target angle that we are rotating towards
    private float targetAngle;
    // how much angle have we rotated (during a rotation)
    private float rotatedAngle;
    // the level 1 object
    private Transform level1;

    // is it allowed to rotate the level?
    // for implementers of collectable: set this to true to allow rotation
    public bool CanRotate { get; set; }

    // initialize the state of our level rotation
    void Start()
    {
        level1 = GameObject.Find("Level1").transform;
        rotating = false;
        initial = true;
        CanRotate = true;
    }

    // rotate on demand in a single update cycle
    void Update()
    {
        // check if the collectable object has disappeared
        // and if so, trigger a rotation
        if (initial)
        {
            var collectable = GameObject.FindWithTag("crystal");
            if (collectable == null) Rotate();
        }

        // rotate the level 1 object
        if (rotating)
        {
            // find the player
            var player = GameObject.Find("Player");
            // angle to rotate in a single update cycle
            var updateAngle = ANGULAR_VELOCITY * Time.deltaTime;
            // stop the player from moving
            player.GetComponent<PlayerController>().KillMovement();
            rotatedAngle -= updateAngle;
            if (rotatedAngle < targetAngle) rotatedAngle = targetAngle;
            level1.rotation = Quaternion.Euler(0f, rotatedAngle, 0f);
        }

        // flip the rotation-in-progress indicator once rotation is finished
        if (Math.Abs(targetAngle - rotatedAngle) < 0.1f && rotating)
            rotating = false;

        // set the active state depending on whether in initial position or not
        ActivateBlocks(initial);
    }

    // set up a rotation
    private void Rotate()
    {
        // initialized the rotated angle variable with current y rotation
        rotatedAngle = level1.eulerAngles.y;

        // set the target angle to zero
        targetAngle = 0f;

        // flip the position state
        initial = !initial;

        // start a rotation
        rotating = true;
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
    private void ActivateSingleBlock(GameObject i)
    {
        i.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
        foreach (var j in i.transform.GetComponent<Walkable>().possiblePath)
            j.active = true;
    }
}
