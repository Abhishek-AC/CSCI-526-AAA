using System;
using UnityEngine;

// preserve and restore the game state of level 1
public class LevelOne : MonoBehaviour
{
    // a data structure for serializing the game state
    [Serializable]
    public class LevelOneState : SaveSystem.GameState
    {
        // is the level in rotated state or initial state
        public bool IsInitialState { get; set; }

        // the position of the player
        public float[] PlayerPosition { get; set; }

        // the rotation of the player
        public float[] PlayerRotation { get; set; }
    }

    // initial rotation state
    private static readonly bool ROTATING_STATE = false;
    // initial level setup state
    private static readonly bool INITIAL_STATE = true;
    // initial player position
    private static readonly float[] INITIAL_PLAYER_POSITION = new float[] { 3.904987f, 0.533f, 0.700345f };
    // initial player rotation
    private static readonly float[] INITIAL_PLAYER_ROTATION = new float[] { 0f, -105.2f, 0f };
    // initial level y angle
    private static readonly float INITIAL_Y_ANGLE = 105.2f;
    // rotated level y angle
    private static readonly float ROTATED_Y_ANGLE = 0f;

    // is the level in rotated state or initial state
    public bool IsInitialState { get; set; }

    // the position of the player
    public float[] PlayerPosition { get; set; }

    // the rotation of the player
    public float[] PlayerRotation { get; set; }

    // the current state of the game
    public LevelOneState CurrentState => new LevelOneState()
    {
        IsInitialState = IsInitialState,
        PlayerPosition = PlayerPosition,
        PlayerRotation = PlayerRotation
    };

    // game setup and restoration
    void Start()
    {
        var restore = SaveSystem.LoadLevelOne();

        // load initial values if there is no state to restore
        IsInitialState = restore
            == null ? INITIAL_STATE : restore.IsInitialState;
        PlayerPosition = restore
            == null ? INITIAL_PLAYER_POSITION : restore.PlayerPosition;
        PlayerRotation = restore
            == null ? INITIAL_PLAYER_ROTATION : restore.PlayerRotation;

        SetPlayerPositionAndRotation();
        SetLevelOneState();
    }

    // update the current game state
    void Update()
    {
        UpdateLevelOneState();
        UpdatePlayerPositionAndRotation();
    }

    // set the level state
    private void SetLevelOneState()
    {
        var level1 = GameObject.Find("Level1");
        var rotator = GameObject.Find("LevelOneRotator");
        if (level1 != null)
            level1.transform.eulerAngles = new Vector3(
                0f, IsInitialState ? INITIAL_Y_ANGLE : ROTATED_Y_ANGLE, 0f);
        if (rotator != null)
            rotator.GetComponent<RotateLevelOne>().Rotating = ROTATING_STATE;
        if (!IsInitialState)
        {
            var collectable = GameObject.FindWithTag("crystal");
            if (collectable != null) collectable.SetActive(false);
        }
    }

    // set the player position and rotation
    private void SetPlayerPositionAndRotation()
    {
        var player = GameObject.Find("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(
                PlayerPosition[0], PlayerPosition[1], PlayerPosition[2]);

            player.transform.eulerAngles = new Vector3(
                PlayerRotation[0], PlayerRotation[1], PlayerRotation[2]);
        }
    }

    // update the level state
    private void UpdateLevelOneState()
    {
        var rotator = GameObject.Find("LevelOneRotator");
        if (rotator != null)
            IsInitialState = rotator.GetComponent<RotateLevelOne>().Initial;
    }

    // update the player position and rotation
    private void UpdatePlayerPositionAndRotation()
    {
        var player = GameObject.Find("Player");
        if (player != null)
        {
            PlayerPosition = new float[]
            {
                player.transform.position.x,
                player.transform.position.y,
                player.transform.position.z
            };

            PlayerRotation = new float[]
            {
                player.transform.eulerAngles.x,
                player.transform.eulerAngles.y,
                player.transform.eulerAngles.z
            };
        }
    }
}
