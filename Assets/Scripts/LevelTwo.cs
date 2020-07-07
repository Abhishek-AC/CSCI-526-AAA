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

        // the rotation angle of the rotatable object
        public float RotateGameObjectRotationZValue { get; set; }

        // the x position of the ferry block
        public float FerryGameObjectPositionXValue { get; set; }

        // the y position of the obstacle/path block
        public float MakePathCubePositionYValue { get; set; }

        // whether the crystal collectable is still present
        public bool IsCrystalPresent { get; set; }

    }

    // invisible gear scale
    private static readonly Vector3 INVISIBLE_GEAR_SCALE = new Vector3(0f, 0f, 0f);
    // visible gear scale
    private static readonly Vector3 VISIBLE_GEAR_SCALE = new Vector3(1f, 1f, 1f);

    // the position of the player capsule
    public float[] PlayerPosition { get; set; }

    // whether the crystal collectable is collected or not
    public bool IsCrystalPresent { get; set; }

    // the rotation angle of the rotatable object
    public float RotateGameObjectRotationZValue { get; set; }

    // the x position of the ferry block
    public float FerryGameObjectPositionXValue { get; set; }

    // the y position of the obstacle/path block
    public float MakePathCubePositionYValue { get; set; }

    // the current state of the game
    public new LevelTwoState CurrentState => new LevelTwoState()
    {
        PlayerPosition = PlayerPosition,
        IsCrystalPresent = IsCrystalPresent,
        RotateGameObjectRotationZValue = RotateGameObjectRotationZValue,
        FerryGameObjectPositionXValue = FerryGameObjectPositionXValue,
        MakePathCubePositionYValue = MakePathCubePositionYValue,
    };

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
        GetPlayerPosition();
        GetCollectableBoolValue();
        GetFerryObject();
        GetMakePathCubeObject();
        GetRotateGameObject();
    }

    // update the current player position
    private void GetPlayerPosition()
    {
        PlayerPosition = new float[3];
        PlayerPosition[0] = CapsulePlayer.transform.position.x;
        PlayerPosition[1] = CapsulePlayer.transform.position.y;
        PlayerPosition[2] = CapsulePlayer.transform.position.z;
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
        // it is interlocked with the state of the collectable object
        RotationGear.transform.localScale = isCrystalPresent ? INVISIBLE_GEAR_SCALE : VISIBLE_GEAR_SCALE;
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
        Vector3 ferryPosition;
        ferryPosition.x = ferryGameObjectPositionXValue;
        ferryPosition.y = 0f;
        ferryPosition.z = 1f;
        ferryObject.transform.position = ferryPosition;
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
        Vector3 makePathCoordinates;
        makePathCoordinates.y = makePathCubePositionYValue;
        makePathCoordinates.x = -1f;
        makePathCoordinates.z = 6f;
        makePathObject.transform.position = makePathCoordinates;
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
        Vector3 rotationAngles;
        rotationAngles.z = rotateGameObjectRotationZValue;
        rotationAngles.x = 0f;
        rotationAngles.y = 0f;
        rotateGameObject.transform.eulerAngles = rotationAngles;
    }

    // set the position of the player
    private void SetPlayerPosition(float[] playerPosition)
    {
        CapsulePlayer = GameObject.Find("Player");
        Vector3 setCapsulePosition;
        setCapsulePosition.x = playerPosition[0];
        setCapsulePosition.y = playerPosition[1];
        setCapsulePosition.z = playerPosition[2];
        CapsulePlayer.transform.position = setCapsulePosition;
    }

    // set the state of the game
    private void SetLevelState(LevelTwoState state)
    {
        SetIsCollectedCrsytal(state.IsCrystalPresent);
        SetFerryGameObject(state.FerryGameObjectPositionXValue);
        SetMakePathGameObject(state.MakePathCubePositionYValue);
        SetRotateGameObject(state.RotateGameObjectRotationZValue);
        SetPlayerPosition(state.PlayerPosition);
    }

    private void InitialLevelState()
    {
        IsCrystalPresent = true;
        RotateGameObjectRotationZValue = 0f;
        FerryGameObjectPositionXValue = -5f;
        MakePathCubePositionYValue = 9f;
        PlayerPosition = new float[] { 0f, 1f, 2f };
        CapsulePlayer.transform.position = new Vector3(PlayerPosition[0], PlayerPosition[1], PlayerPosition[2]);
        RotationGear.transform.localScale = INVISIBLE_GEAR_SCALE;
    }

    // save the current game state to file
    public override void SaveLevel()
    {
        SaveSystem.SaveLevelTwo(this);
    }

    // unused: load the game state from save file
    public override void LoadLevel()
    {
        LevelTwoState data = SaveSystem.LoadLevelTwo();
        if (data != null) SetLevelState(data);
    }

    // reset the game state
    public override void ResetLevel()
    {
        InitialLevelState();
        SaveLevel();
    }
}
