using System;
using UnityEngine;

// preserve and restore the game state of level 2
public class LevelTwo : LevelManager
{
    // a data structure for serializing the game state
    [Serializable]
    public class LevelTwoState : SaveSystem.GameState
    {
        // the position of the player capsule
        public float[] PlayerPosition { get; set; }
        
        // the rotation of the player capsule
        public float[] PlayerRotation { get; set; }

        // the rotation angle of the rotatable object
        public float RotateGameObjectRotationZValue { get; set; }

        // the x position of the ferry block
        public float FerryGameObjectPositionXValue { get; set; }

        // the y position of the obstacle/path block
        public float MakePathCubePositionYValue { get; set; }

        // whether the crystal collectable is still present
        public bool IsCrystalPresent { get; set; }

    }

    private static readonly bool INITIAL_CRYSTAL_PRESENCE = true;
    // invisible gear scale
    private static readonly Vector3 INVISIBLE_GEAR_SCALE = new Vector3(0f, 0f, 0f);
    // visible gear scale
    private static readonly Vector3 VISIBLE_GEAR_SCALE = new Vector3(1f, 1f, 1f);
    // initial player position
    private static readonly float[] INITIAL_PLAYER_POSITION = new[] { 0f, 1f, 2f };
    // initial player rotation
    private static readonly float[] INITIAL_PLAYER_ROTATION = new[] { 0f, 0f, 0f };
    private static readonly float INITIAL_ROTATE_Z_VALUE = 0f;
    private static readonly float INITIAL_FERRY_X_VALUE = -5f;
    private static readonly float INITIAL_MAKEPATH_Y_VALUE = 9f;

    // the position of the player capsule
    public float[] PlayerPosition { get; set; }
    
    // the rotation of the player capsule
    public float[] PlayerRotation { get; set; }

    // whether the crystal collectable is collected or not
    public bool IsCrystalPresent { get; set; }

    // the rotation angle of the rotatable object
    public float RotateGameObjectRotationZValue { get; set; }

    // the x position of the ferry block
    public float FerryGameObjectPositionXValue { get; set; }

    // the y position of the obstacle/path block
    public float MakePathCubePositionYValue { get; set; }

    // the current state of the game
    public new LevelTwoState CurrentState
    {
        get => new LevelTwoState()
        {
            PlayerPosition = PlayerPosition,
            PlayerRotation = PlayerRotation,
            IsCrystalPresent = IsCrystalPresent,
            RotateGameObjectRotationZValue = RotateGameObjectRotationZValue,
            FerryGameObjectPositionXValue = FerryGameObjectPositionXValue,
            MakePathCubePositionYValue = MakePathCubePositionYValue,
        };
    }

    // frequently used game objects
    private GameObject CapsulePlayer;
    private GameObject Collectable;
    private GameObject RotationGear;

    // game setup and restoration
    void Start()
    {
        var data = SaveSystem.LoadLevelTwo();
        CapsulePlayer = GameObject.Find("Player");
        Collectable = GameObject.Find("Collectable");
        RotationGear = GameObject.Find("RotationGear");

        // load initial values if there is no state to restore
        if (data != null)
            SetLevelState(data);
        else
            InitialLevelState();
    }

    // update the current game state
    void Update()
    {
        GetPlayerPositionAndRotation();
        GetCollectableBoolValue();
        GetFerryObject();
        GetMakePathCubeObject();
        GetRotateGameObject();
    }

    // update the current player position and rotation
    private void GetPlayerPositionAndRotation()
    {
        PlayerPosition = new[]
        {
            CapsulePlayer.transform.position.x,
            CapsulePlayer.transform.position.y,
            CapsulePlayer.transform.position.z
        };
        PlayerRotation = new[]
        {
            CapsulePlayer.transform.eulerAngles.x,
            CapsulePlayer.transform.eulerAngles.y,
            CapsulePlayer.transform.eulerAngles.z
        };
    }

    // update the state of the crystal collectable
    private void GetCollectableBoolValue() => IsCrystalPresent
        = Collectable != null && Collectable.activeSelf;

    // set the state of the crystal collectable and the interlocked gear
    private void SetIsCollectedCrsytal(bool isCrystalPresent)
    {
        if (Collectable != null)
            Collectable.SetActive(isCrystalPresent);
        // also update the visibility state of the rotation gear
        // and the presence of the animator component of the rotate block
        // they are interlocked with the state of the collectable object
        RotationGear.transform.localScale = isCrystalPresent ? INVISIBLE_GEAR_SCALE : VISIBLE_GEAR_SCALE;
        if (!isCrystalPresent &&
            GameObject.Find("Rotate") != null &&
            GameObject.Find("Rotate").GetComponent<Animator>() != null)
            Destroy(GameObject.Find("Rotate").GetComponent<Animator>());
            
    }

    // get the position of the ferry block
    private void GetFerryObject()
    {
        GameObject ferryObject = GameObject.Find("move");
        FerryGameObjectPositionXValue = ferryObject.transform.position.x;
    }

    // set the position of the ferry block
    private void SetFerryGameObject(float ferryGameObjectPositionXValue)
    {
        GameObject ferryObject = GameObject.Find("move");
        // remove the offending animations
        if (GameObject.Find("move") != null && GameObject.Find("move").GetComponent<Animator>() != null)
            Destroy(GameObject.Find("move").GetComponent<Animator>());
        ferryObject.transform.position = new Vector3(ferryGameObjectPositionXValue, 0f, 1f);
    }

    // get the position of the obstacle/path block
    private void GetMakePathCubeObject()
    {
        GameObject makePathCubeObject = GameObject.Find("MakePath");
        MakePathCubePositionYValue = makePathCubeObject.transform.position.y;
    }

    // set the position of the obstacle/path block
    private void SetMakePathGameObject(float makePathCubePositionYValue)
    {
        GameObject makePathObject = GameObject.Find("MakePath");
        makePathObject.transform.position = new Vector3(-1f, makePathCubePositionYValue, 6f);
    }

    // get the rotation angle of the rotatable object
    private void GetRotateGameObject()
    {
        GameObject rotateGameObject = GameObject.Find("Rotate");
        RotateGameObjectRotationZValue = rotateGameObject.transform.rotation.eulerAngles.z;
    }

    // set the rotation angle of the rotatable object
    private void SetRotateGameObject(float rotateGameObjectRotationZValue)
    {
        GameObject rotateGameObject = GameObject.Find("Rotate");
        rotateGameObject.transform.eulerAngles = new Vector3(0f, 0f, rotateGameObjectRotationZValue);
    }

    // set the position of the player
    private void SetPlayerPositionAndRotation(float[] playerPosition, float[] playerRotation)
    {
        CapsulePlayer = GameObject.Find("Player");
        CapsulePlayer.transform.position = new Vector3(playerPosition[0], playerPosition[1], playerPosition[2]);
        CapsulePlayer.transform.eulerAngles = new Vector3(playerRotation[0], playerRotation[1], playerRotation[2]);
    }

    // set the state of the game
    private void SetLevelState(LevelTwoState state)
    {
        SetIsCollectedCrsytal(state.IsCrystalPresent);
        SetFerryGameObject(state.FerryGameObjectPositionXValue);
        SetMakePathGameObject(state.MakePathCubePositionYValue);
        SetRotateGameObject(state.RotateGameObjectRotationZValue);
        SetPlayerPositionAndRotation(state.PlayerPosition, state.PlayerRotation);
    }

    private void InitialLevelState()
    {
        IsCrystalPresent = INITIAL_CRYSTAL_PRESENCE;
        RotateGameObjectRotationZValue = INITIAL_ROTATE_Z_VALUE;
        FerryGameObjectPositionXValue = INITIAL_FERRY_X_VALUE;
        MakePathCubePositionYValue = INITIAL_MAKEPATH_Y_VALUE;
        PlayerPosition = INITIAL_PLAYER_POSITION;
        PlayerRotation = INITIAL_PLAYER_ROTATION;
        RotationGear.transform.localScale = INVISIBLE_GEAR_SCALE;
        CapsulePlayer.transform.position = new Vector3(PlayerPosition[0], PlayerPosition[1], PlayerPosition[2]);
        CapsulePlayer.transform.eulerAngles = new Vector3(PlayerRotation[0], PlayerRotation[1], PlayerRotation[2]);
    }

    // save the current game state to file
    public override void SaveLevel() => SaveSystem.SaveLevelTwo(this);

    // unused: load the game state from save file
    public override void LoadLevel()
    {
        LevelTwoState data = SaveSystem.LoadLevelTwo();
        if (data != null) SetLevelState(data);
    }

    // reset the game state
    public override void ResetLevel() => SaveSystem.ResetLevelTwo();
}
