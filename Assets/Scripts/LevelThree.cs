using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelThree : LevelManager
{
    [Serializable]
    public class LevelThreeState : SaveSystem.GameState
    {
        public float[] PlayerPosition { get; set; }

        public float[] PlayerRotation { get; set; }

        public float RotateDestinationAngle { get; set; }

        public float RotateKeyAngle { get; set; }

        public Dictionary<string, bool> SelfDestructableBlocksStatus { get; set; }

        public bool IsCrystalTwoPresent { get; set; }

        public bool IsRotationKeyPresent { get; set; }
    }

    // initial player position
    private static readonly float[] INITIAL_PLAYER_POSITION = new float[] { -4f, 4.59f, -3f };
    // initial player rotation
    private static readonly float[] INITIAL_PLAYER_ROTATION = new float[] { 0f, 0f, 0f };
    // initial rotation angle of the destination object
    private static readonly float INITIAL_ROTATE_DESTINATION_ANGLE = 0f;
    // initial rotation angle of the key object
    private static readonly float INITIAL_ROTATE_KEY_ANGLE = 270f;
    // initial self-destruct blocks status
    private static readonly Dictionary<string, bool> INITIAL_SELF_DESTRUCTABLE_BLOCKS_STATUS
        = new Dictionary<string, bool>()
        {
            { "Cube (10)", true },
            { "Cube (11)", true },
            { "Cube (12)", true },
            { "Cube (13)", true },
            { "Cube (14)", true }
        };
    // initial crystal2 presence status
    private static readonly bool INITIAL_IS_CRYSTAL_TWO_PRESENT = true;
    // initial rotation_key presence status
    private static readonly bool INITIAL_IS_ROTATION_KEY_PRESENT = true;
    // invisible gear scale
    private static readonly Vector3 INVISIBLE_GEAR_SCALE = new Vector3(0f, 0f, 0f);
    // visible gear scale
    private static readonly Vector3 VISIBLE_GEAR_SCALE = new Vector3(1f, 1f, 1f);

    // do we want to reset the game
    private bool reset = false;

    public float[] PlayerPosition { get; set; }

    public float[] PlayerRotation { get; set; }

    public float RotateDestinationAngle { get; set; }

    public float RotateKeyAngle { get; set; }

    public Dictionary<string, bool> SelfDestructableBlocksStatus { get; set; }

    public bool IsCrystalTwoPresent { get; set; }

    public bool IsRotationKeyPresent { get; set; }

    public new LevelThreeState CurrentState => new LevelThreeState()
    {
        PlayerPosition = PlayerPosition,
        PlayerRotation = PlayerRotation,
        RotateDestinationAngle = RotateDestinationAngle,
        RotateKeyAngle = RotateKeyAngle,
        SelfDestructableBlocksStatus = SelfDestructableBlocksStatus,
        IsCrystalTwoPresent = IsCrystalTwoPresent,
        IsRotationKeyPresent = IsRotationKeyPresent
    };

    // game setup and restoration
    void Start() => RestoreOrSetupGameState();

    // update the current game state
    void Update()
    {
        var rotationGearKey = GameObject.Find("RotationGear_Key");
        var rotationGearDestination = GameObject.Find("RotationGear_Destination");
        Debug.Log($"Updated values: RotationGear_Key: {rotationGearKey.transform.localScale} RotationGear_Destination: {rotationGearDestination.transform.localScale}");
        UpdatePlayerPositionAndRotation();
        UpdateCollectablesStatus();
        UpdateRotateAngles();
        UpdateSelfDestructableBlocksStatus();
    }

    // unused: load game state from file
    public override void LoadLevel() => RestoreOrSetupGameState();

    // save the current game state to file
    public override void SaveLevel() => SaveSystem.SaveLevelThree(this);

    // reset the game state
    public override void ResetLevel()
    {
        reset = true;
        RestoreOrSetupGameState();
        SaveLevel();
    }

    private void RestoreOrSetupGameState()
    {
        var state = reset ? null : SaveSystem.LoadLevelThree();
        // load initial values if there is no state to restore
        PlayerPosition = state
            == null ? INITIAL_PLAYER_POSITION : state.PlayerPosition;
        PlayerRotation = state
            == null ? INITIAL_PLAYER_ROTATION : state.PlayerRotation;
        RotateDestinationAngle = state
            == null ? INITIAL_ROTATE_DESTINATION_ANGLE : state.RotateDestinationAngle;
        RotateKeyAngle = state
            == null ? INITIAL_ROTATE_KEY_ANGLE : state.RotateKeyAngle;
        SelfDestructableBlocksStatus = state
            == null ? INITIAL_SELF_DESTRUCTABLE_BLOCKS_STATUS : state.SelfDestructableBlocksStatus;
        IsCrystalTwoPresent = state
            == null ? INITIAL_IS_CRYSTAL_TWO_PRESENT : state.IsCrystalTwoPresent;
        IsRotationKeyPresent = state
            == null ? INITIAL_IS_ROTATION_KEY_PRESENT : state.IsRotationKeyPresent;

        SetPlayerPositionAndRotation();
        SetCollectablesStatus();
        SetRotateAngles();
        SetSelfDestructableBlocksStatus();
    }

    // set the player position and rotation - local coordinates relative to level3 object
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

    // set the status of the collectables and the interlocked gears
    private void SetCollectablesStatus()
    {
        var crystalTwo = GameObject.Find("cristal_2");
        var rotationKey = GameObject.Find("key");
        var rotationGearKey = GameObject.Find("RotationGear_Key");
        var rotationGearDestination = GameObject.Find("RotationGear_Destination");

        // set the status of the crystal and the interlocked rotation gear
        if (crystalTwo != null) crystalTwo.SetActive(IsCrystalTwoPresent);
        if (rotationGearKey != null) rotationGearKey
                .transform.localScale = IsCrystalTwoPresent ?
                INVISIBLE_GEAR_SCALE : VISIBLE_GEAR_SCALE;

        // set the status of the rotation key and the interlocked rotation gear
        if (rotationKey != null) rotationKey.SetActive(IsRotationKeyPresent);
        if (rotationGearDestination != null) rotationGearDestination
                .transform.localScale = IsRotationKeyPresent ?
                INVISIBLE_GEAR_SCALE : VISIBLE_GEAR_SCALE;

        Debug.Log($"Initialized values: RotationGear_Key: {rotationGearKey.transform.localScale} RotationGear_Destination: {rotationGearDestination.transform.localScale}");
    }

    // set the angles of the rotate objects
    private void SetRotateAngles()
    {
        var rotateKey = GameObject.Find("Rotate_Key");
        var rotateDestination = GameObject.Find("Rotate_Destination");
        rotateKey.transform.eulerAngles = new Vector3(0f, 0f, RotateKeyAngle);
        rotateDestination.transform.eulerAngles = new Vector3(RotateDestinationAngle, 0f, 0f);
    }

    // set the status for the self-destructable blocks
    private void SetSelfDestructableBlocksStatus()
    {
        foreach (var i in SelfDestructableBlocksStatus.Keys.ToArray())
        {
            var status = SelfDestructableBlocksStatus[i];
            var block = GameObject.Find(i);
            if (block != null)
            {
                if (!status)
                    block.GetComponent<SelfDestructable>().DisableNeighborPaths();
                block.SetActive(status);
            }
        }
    }

    // update the player position and rotation - local coordinates relative to level3 object
    private void UpdatePlayerPositionAndRotation()
    {
        var player = GameObject.Find("Player");
        if (player != null)
        {
            PlayerPosition = new float[]
            {
                player.transform.localPosition.x,
                player.transform.localPosition.y,
                player.transform.localPosition.z
            };

            PlayerRotation = new float[]
            {
                player.transform.localEulerAngles.x,
                player.transform.localEulerAngles.y,
                player.transform.localEulerAngles.z
            };
        }
    }

    // update the status of the collectables
    private void UpdateCollectablesStatus()
    {
        var crystalTwo = GameObject.Find("cristal_2");
        var rotationKey = GameObject.Find("key");
        IsCrystalTwoPresent = crystalTwo != null;
        IsRotationKeyPresent = rotationKey != null;
    }

    // update the angles of the rotate objects
    private void UpdateRotateAngles()
    {
        var rotateKey = GameObject.Find("Rotate_Key");
        var rotateDestination = GameObject.Find("Rotate_Destination");
        RotateKeyAngle = rotateKey.transform.eulerAngles.z;
        RotateDestinationAngle = rotateDestination.transform.eulerAngles.x;
    }

    // update the status for the self-destructable blocks
    private void UpdateSelfDestructableBlocksStatus() =>
        SelfDestructableBlocksStatus =
            SelfDestructableBlocksStatus
            .ToDictionary(kv => kv.Key,
                kv => GameObject.Find(kv.Key) != null
                && !GameObject.Find(kv.Key)
                .GetComponent<SelfDestructable>()
                .IsSelfDestructionTriggered);
}
