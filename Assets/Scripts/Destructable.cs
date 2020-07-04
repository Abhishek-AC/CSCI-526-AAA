using UnityEngine;

// makes a block as destructable
// self-destructs after the player passes
public class Destructable : MonoBehaviour
{
    // hard-coded self-destruct delay time
    public static readonly float SELF_DESTRUCT_DELAY = 1f;

    // is the player currently on this blocks
    // dynamic property: calculated at runtime
    public bool IsPlayerOnThisBlock
    {
        get => GameObject.Find("Player") != null &&
            Physics.Raycast(transform.position,
                GameObject.Find("Player").transform.up, 10f);
    }

    // whether the player has entered the block
    public bool PlayerHasEntered { get; private set; }

    // whether the player has left the block
    public bool PlayerHasLeft { get; private set; }

    // the self-destruct delay countdown timer
    private float timer;

    // initialize the known good state
    void Start()
    {
        PlayerHasEntered = false;
        PlayerHasLeft = false;
        timer = SELF_DESTRUCT_DELAY;
    }

    // update the player status flags and optionally trigger self-destruct
    void Update()
    {
        SetPlayerStateFlags();
        SelfDestructCountdown();
    }

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

    // self-destruction logic
    private void SelfDestruct()
    {
        // update the possible paths of neighboring blocks
        NeighborPathDisableDriver();

        // add a Rigidbody component (uses gravity by default)
        // this will cause the block to fall due to gravity
        if (transform.GetComponent<Rigidbody>() == null)
            transform.gameObject.AddComponent<Rigidbody>();
    }

    // determine whether to self-destruct based on player status and countdown
    private void SelfDestructCountdown()
    {
        // once the player has left, trigger the countdown
        // once the countdown reaches zero, trigger self-destruction
        if (PlayerHasLeft)
        {
            // if the delay timer reaches zero
            // trigger self-destruction
            if (timer <= 0f) SelfDestruct();
            // or continue to countdown
            else
                timer = timer - Time.deltaTime <= 0f ?
                    0f : timer - Time.deltaTime;
        }
    }

    // common path disabling logic
    private void NeighborPathDisable(GameObject neighbor, int index) => neighbor
        .GetComponent<Walkable>()
        .possiblePath[index]
        .active = false;

    // block-specific, hard-coded logic on disableing neighbor paths
    // only contains the paths to be disabled for each self-destruct block
    // actual disable logic is done in NeighborPathDisable() function
    private void NeighborPathDisableDriver()
    {

    }
}
