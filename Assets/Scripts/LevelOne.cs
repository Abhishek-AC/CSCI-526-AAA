using System;
using UnityEngine;

// preserve and restore the game state of level 1
public class LevelOne : LevelManager
{
    // a data structure for serializing the game state
    [Serializable]
    public class LevelOneState : SaveSystem.GameState
    {
        // is the collectable present or not
        public bool IsCollectablePresent { get; set; }
        
        // the rotation angle of level one
        public float LevelOneRotation { get; set; }

        // the position of the player
        public float[] PlayerPosition { get; set; }

        // the rotation of the player
        public float[] PlayerRotation { get; set; }
    }
    
    // initial collectable presence
    private static readonly bool INITIAL_COLLECTABLE_PRESENCE = true;
    // initial player position - local coordinates relative to level1 object
    private static readonly float[] INITIAL_PLAYER_POSITION = new[] { 3.904987f, 0.533f, 0.700345f };
    // initial player rotation - local coordinates relative to level1 object
    private static readonly float[] INITIAL_PLAYER_ROTATION = new[] { 0f, -105.2f, 0f };
    // initial level y angle
    private static readonly float INITIAL_Y_ANGLE = 105.2f;

    // do we want to reset the game
    private bool reset = false;

    // is the collectable present or not
    public bool IsCollectablePresent { get; set; }
    
    // the rotation angle of level one
    public float LevelOneRotation { get; set; }

    // the position of the player
    public float[] PlayerPosition { get; set; }

    // the rotation of the player
    public float[] PlayerRotation { get; set; }

    // the current state of the game
    public new LevelOneState CurrentState
    {
        get => new LevelOneState()
        {
            IsCollectablePresent =  IsCollectablePresent,
            LevelOneRotation = LevelOneRotation,
            PlayerPosition = PlayerPosition,
            PlayerRotation = PlayerRotation
        };
    }

    // game setup and restoration
    void Start() => RestoreOrSetupGameState();

    // update the current game state
    void Update()
    {
        UpdateCollectablePresence();
        UpdateLevelOneRotationAngle();
        UpdatePlayerPositionAndRotation();
    }

    // unused: load game state from file
    public override void LoadLevel() => RestoreOrSetupGameState();

    // save the current game state to file
    public override void SaveLevel() => SaveSystem.SaveLevelOne(this);

    // reset the game state
    public override void ResetLevel()
    {
        reset = true;
        RestoreOrSetupGameState();
        SaveLevel();
    }

    // restore the game state or set up in its initial state
    private void RestoreOrSetupGameState()
    {
        var state = reset ? null : SaveSystem.LoadLevelOne();
        // load initial values if there is no state to restore
        IsCollectablePresent = state?.IsCollectablePresent ?? INITIAL_COLLECTABLE_PRESENCE;
        LevelOneRotation = state?.LevelOneRotation ?? INITIAL_Y_ANGLE;
        PlayerPosition = state?.PlayerPosition ?? INITIAL_PLAYER_POSITION;
        PlayerRotation = state?.PlayerRotation ?? INITIAL_PLAYER_ROTATION;

        SetCollectablePresence();
        SetPlayerPositionAndRotation();
        SetLevelOneRotationAngle();
    }
    
    // set the collectable status
    private void SetCollectablePresence()
    {
        var collectable = GameObject.Find("Collectable");
        if (collectable != null) collectable.SetActive(IsCollectablePresent);
    }

    // set the level one rotation angle
    private void SetLevelOneRotationAngle()
    {
        var level1 = GameObject.Find("Level1");
        if (level1 != null)
            level1.transform.eulerAngles = new Vector3(0f, LevelOneRotation, 0f);
    }

    // set the player position and rotation - local coordinates relative to level1 object
    private void SetPlayerPositionAndRotation()
    {
        var player = GameObject.Find("Player");
        if (player != null)
        {
            player.transform.localPosition = new Vector3(
                PlayerPosition[0], PlayerPosition[1], PlayerPosition[2]);

            player.transform.localEulerAngles = new Vector3(
                PlayerRotation[0], PlayerRotation[1], PlayerRotation[2]);
        }
    }
    
    // update the collectable presence
    private void UpdateCollectablePresence() => IsCollectablePresent = GameObject.Find("Collectable") != null;

    // update the level one rotation angle
    private void UpdateLevelOneRotationAngle() => LevelOneRotation = GameObject.Find("Level1").transform.rotation.eulerAngles.y;

    // update the player position and rotation - local coordinates relative to level1 object
    private void UpdatePlayerPositionAndRotation()
    {
        var player = GameObject.Find("Player");
        if (player != null)
        {
            PlayerPosition = new[]
            {
                player.transform.localPosition.x,
                player.transform.localPosition.y,
                player.transform.localPosition.z
            };

            PlayerRotation = new[]
            {
                player.transform.localEulerAngles.x,
                player.transform.localEulerAngles.y,
                player.transform.localEulerAngles.z
            };
        }
    }
}
