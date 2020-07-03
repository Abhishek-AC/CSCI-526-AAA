using UnityEngine;

// makes a block as destroyable
// self-destroys after the player passes
public class Destroyable : MonoBehaviour
{
    public static readonly float SELF_DESTROY_DELAY = 1.5f;

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

    // the self-destroy delay timer
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHasEntered = false;
        PlayerHasLeft = false;
        timer = SELF_DESTROY_DELAY;
    }

    // Update is called once per frame
    void Update()
    {
        SetPlayerStateFlags();
        SelfDestroy();
    }

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

    private void SelfDestroy()
    {
        // once the player has left, trigger the countdown
        // once the countdown reaches zero, trigger self-destroy
        if (PlayerHasLeft)
        {
            // if the delay timer reaches zero
            // trigger self destroy
            if (timer <= 0f) transform.gameObject.SetActive(false);
            // or continue to countdown
            else
                timer = timer - Time.deltaTime <= 0f ?
                    0f : timer - Time.deltaTime;
        }
    }
}
