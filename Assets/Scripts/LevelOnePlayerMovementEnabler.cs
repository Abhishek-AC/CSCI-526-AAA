using System;
using UnityEngine;

public class LevelOnePlayerMovementEnabler : MonoBehaviour
{
    // the initial rotation angle
    private static readonly float INITIAL_Y_ANGLE = 105.2f;
    // the "snapped" rotation angle
    private static readonly float ROTATED_Y_ANGLE = 0f;
    
    // the player object
    private GameObject player;
    
    // initialize instance variables
    void Start() => player = GameObject.Find("Player");

    // update whether the player is allowed to move depending on rotation angle
    void Update()
    {
        if (player != null)
            // the player is only allowed to move at initial or "snapped" rotation angles
            if (Math.Abs(transform.eulerAngles.y - INITIAL_Y_ANGLE) <= 0.1f ||
                Math.Abs(transform.eulerAngles.y - ROTATED_Y_ANGLE) <= 0.1f)
                player.GetComponent<PlayerController>().CanMove = true;
            else
                player.GetComponent<PlayerController>().CanMove = false;
    }
}
