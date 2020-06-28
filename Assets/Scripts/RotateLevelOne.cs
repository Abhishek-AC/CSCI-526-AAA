using System;
using UnityEngine;

public class RotateLevelOne : MonoBehaviour
{
    // the y rotation angle for the initial position
    private static readonly float INITIAL_Y_ANGLE = 104f;
    // the y rotation angle for the trasformed position
    private static readonly float ROTATED_Y_ANGLE = 360f;
    // the speed of rotation defined as angle per second
    private static readonly float ANGULAR_VELOCITY = 45f;

    // have we activated self-destroy logic yet
    private bool trippedSelfDestroy;
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
        trippedSelfDestroy = false;
        rotating = false;
        initial = true;
        CanRotate = true;
    }

    // rotate on demand in a single update cycle
    void Update()
    {
        // rotate the level 1 object
        if (rotating)
        {
            // angle to rotate in a single update cycle
            var updateAngle = ANGULAR_VELOCITY * Time.deltaTime;
            rotatedAngle += updateAngle;
            if (rotatedAngle > targetAngle) rotatedAngle = targetAngle;
            level1.rotation = Quaternion.Euler(0f, rotatedAngle, 0f);
        }

        // flip the rotation-in-progress indicator once rotation is finished
        if (Math.Abs(targetAngle - rotatedAngle) < 0.1f && rotating)
            rotating = false;

        // set the active state depending on whether in initial position or not
        ActivateBlocks(initial);

        // self-destroy once we tripped rotation
        if (trippedSelfDestroy && !rotating)
            transform.gameObject.SetActive(false);
    }

    // event handler for triggering rotation
    private void OnMouseDown()
    {
        if (CanRotate && !rotating)
            Rotate();
    }

    // set up a rotation
    private void Rotate()
    {
        // initialized the rotated angle variable with current y rotation
        rotatedAngle = level1.eulerAngles.y;

        // select target angle depending on whether in initial position or not
        targetAngle = initial ? ROTATED_Y_ANGLE : INITIAL_Y_ANGLE;

        // flip the position state
        initial = !initial;

        // start a rotation
        rotating = true;

        // active self-destroy logic
        trippedSelfDestroy = true;
    }

    // selectively activate the blocks depending on whether we are in the initial position or not
    private void ActivateBlocks(bool isInitial)
    {
        if (isInitial)
        {
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g1"))
                i.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g2"))
                i.transform.GetComponent<Walkable>().canWalkOnThisBlock = false;
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g3"))
                i.transform.GetComponent<Walkable>().canWalkOnThisBlock = false;
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g4"))
                i.transform.GetComponent<Walkable>().canWalkOnThisBlock = false;
        }
        else
        {
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g1"))
                i.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g2"))
                i.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g3"))
                i.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
            foreach (var i in GameObject.FindGameObjectsWithTag("l1g4"))
                i.transform.GetComponent<Walkable>().canWalkOnThisBlock = true;
        }
    }
}
