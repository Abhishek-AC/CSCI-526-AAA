using UnityEngine;

// makes a block self-destruct after the player passes
public class SelfDestructable : MonoBehaviour
{
    // hard-coded self-destruct delay time
    public static readonly float SELF_DESTRUCT_DELAY = 0.75f;

    // whether the player currently is on this blocks or not
    // dynamic property: calculated at runtime
    public bool IsPlayerOnThisBlock
    {
        get => GameObject.Find("Player") != null &&
            Physics.Raycast(transform.position,
                GameObject.Find("Player").transform.up, 10f);
    }

    // whether self-destruction countdown is triggered
    // dynamic property: calculated at runtime
    public bool IsSelfDestructionTriggered
    {
        get => PlayerHasEntered && PlayerHasLeft;
    }

    // whether the player has entered the block or not
    public bool PlayerHasEntered { get; private set; }

    // whether the player has left the block or not
    public bool PlayerHasLeft { get; private set; }

    // the self-destruct delay countdown timer
    private float triggerTimer;

    // initialize the known good state
    void Start()
    {
        PlayerHasEntered = false;
        PlayerHasLeft = false;
        triggerTimer = SELF_DESTRUCT_DELAY;
    }

    // update the player status flags and optionally trigger self-destruct
    void Update()
    {
        SetPlayerStateFlags();
        SelfDestructCountdown();
    }

    // deactivates the block once it becomes invisible
    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    // used by the level 3 manager to quickly disable neighbor paths
    public void DisableNeighborPaths() => NeighborPathDisableDriver();

    // update the player status flags
    private void SetPlayerStateFlags()
    {
        // if the player is on the block
        // then we set PlayerHasEntered
        if (!PlayerHasEntered && IsPlayerOnThisBlock)
            PlayerHasEntered = true;

        // if the player is not on the block
        // and PlayerHasEntered is set
        // then we set PlayerHasLeft
        if (PlayerHasEntered && !IsPlayerOnThisBlock)
            PlayerHasLeft = true;
    }

    // self-destruct logic
    private void SelfDestruct()
    {
        // update the possible paths of neighboring blocks
        NeighborPathDisableDriver();

        // add a Rigidbody component (uses gravity by default)
        // this will cause the block to fall due to gravity
        // and trigger deactivation after falling for a period of time
        if (transform.GetComponent<Rigidbody>() == null)
            transform.gameObject.AddComponent<Rigidbody>();
    }

    // determine whether to self-destruct based on player status and countdown
    // also counts down automatically once the trigger conditions are met
    private void SelfDestructCountdown()
    {
        // once the player has left, trigger the countdown
        // once the countdown reaches zero, trigger self-destruction
        if (PlayerHasLeft)
        {
            // if the delay timer reaches zero
            // trigger self-destruction
            if (triggerTimer <= 0f) SelfDestruct();
            // or continue to countdown
            else
                triggerTimer = triggerTimer - Time.deltaTime <= 0f ?
                    0f : triggerTimer - Time.deltaTime;
        }
    }

    // common neighbor path disabling logic
    private void NeighborPathDisable(string neighbor, int index) => GameObject
        .Find(neighbor)
        .GetComponent<Walkable>()
        .possiblePath[index]
        .active = false;

    // block-specific, hard-coded logic on disableing neighbor paths
    // only contains the paths to be disabled for each self-destruct block
    // actual disable logic is done in NeighborPathDisable() function
    private void NeighborPathDisableDriver()
    {
        switch (transform.gameObject.name)
        {
            case "Cube (10)":
                if (GameObject.Find("Cube (40)") != null) NeighborPathDisable("Cube (40)", 0);
                if (GameObject.Find("Cube (9)") != null) NeighborPathDisable("Cube (9)", 1);
                if (GameObject.Find("Cube (11)") != null) NeighborPathDisable("Cube (11)", 0);
                break;
            case "Cube (11)":
                if (GameObject.Find("Cube (10)") != null) NeighborPathDisable("Cube (10)", 0);
                if (GameObject.Find("Cube (12)") != null) NeighborPathDisable("Cube (12)", 0);
                break;
            case "Cube (12)":
                if (GameObject.Find("Cube (11)") != null) NeighborPathDisable("Cube (11)", 1);
                if (GameObject.Find("Cube (13)") != null) NeighborPathDisable("Cube (13)", 1);
                break;
            case "Cube (13)":
                if (GameObject.Find("Cube (12)") != null) NeighborPathDisable("Cube (12)", 1);
                if (GameObject.Find("Cube (14)") != null) NeighborPathDisable("Cube (14)", 0);
                break;
            case "Cube (14)":
                if (GameObject.Find("Cube (13)") != null) NeighborPathDisable("Cube (13)", 0);
                if (GameObject.Find("Cube (15)") != null) NeighborPathDisable("Cube (15)", 0);
                break;
        }
    }
}
