using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    public bool isCollectedCrystal;
    public float rotateGameObjectRotationZValue;
    public float ferryGameObjectPositionXValue;
    public float makePathCubePositionYValue;
    public float[] capsulePosition;
    GameObject capsulePlayer;
    public GameObject collectable;
    void Start()
    {
        Level2Data data = SaveSystem.LoadLevel();
        if (data != null)
        {
            setIsCollectedCrsytal(data.isCollectedCrystal);
            setFerryGameObject(data.ferryGameObjectPositionXValue);
            setMakePathGameObject(data.makePathCubePositionYValue);
            setRotateGameObject(data.rotateGameObjectRotationZValue);
            setCapsulePosition(data.capsulePosition);
        }
        else
        {
            isCollectedCrystal = true;
            rotateGameObjectRotationZValue = 0f;
            ferryGameObjectPositionXValue = -5f;
            makePathCubePositionYValue = 9f;
            capsulePlayer = GameObject.Find("Player");
            capsulePlayer.transform.position = new Vector3(0f, 1f, 2f);
        }
    }
    void Update()
    {
        getPlayerPosition();
        getCollectableBoolValue();
        getFerryObject();
        getMakePathCubeObject();
        getRotateGameObject();
    }
    public void getPlayerPosition()
    {
        capsulePlayer = GameObject.Find("Player");
        capsulePosition = new float[3];
        capsulePosition[0] = capsulePlayer.transform.position.x;
        capsulePosition[1] = capsulePlayer.transform.position.y;
        capsulePosition[2] = capsulePlayer.transform.position.z;
    }
    public void getCollectableBoolValue()
    {
        isCollectedCrystal = collectable.activeSelf;
    }
    public void setIsCollectedCrsytal(bool isCollectedCrystal)
    {
        GameObject collectableObject = GameObject.Find("Collectable");
        if (collectableObject != null)
        {
            collectableObject.SetActive(isCollectedCrystal);
        }
    }
    public void getFerryObject()
    {
        GameObject ferryObject = GameObject.Find("move");
        ferryGameObjectPositionXValue = ferryObject.transform.position.x;
    }
    public void setFerryGameObject(float ferryGameObjectPositionXValue)
    {
        GameObject ferryObject = GameObject.Find("move");
        Vector3 ferryPosition;
        ferryPosition.x = ferryGameObjectPositionXValue;
        ferryPosition.y = 0f;
        ferryPosition.z = 1f;
        ferryObject.transform.position = ferryPosition;
    }
    public void getMakePathCubeObject()
    {
        GameObject makePathCubeObject = GameObject.Find("MakePath");
        makePathCubePositionYValue = makePathCubeObject.transform.position.y;
    }
    public void setMakePathGameObject(float makePathCubePositionYValue)
    {
        GameObject makePathObject = GameObject.Find("MakePath");
        Vector3 makePathCoordinates;
        makePathCoordinates.y = makePathCubePositionYValue;
        makePathCoordinates.x = -1f;
        makePathCoordinates.z = 6f;
        makePathObject.transform.position = makePathCoordinates;
    }
    public void getRotateGameObject()
    {
        GameObject rotateGameObject = GameObject.Find("Rotate");
        rotateGameObjectRotationZValue = rotateGameObject.transform.rotation.eulerAngles.z;
    }
    public void setRotateGameObject(float rotateGameObjectRotationZValue)
    {
        GameObject rotateGameObject = GameObject.Find("Rotate");
        Vector3 rotationAngles;
        rotationAngles.z = rotateGameObjectRotationZValue;
        rotationAngles.x = 0f;
        rotationAngles.y = 0f;
        rotateGameObject.transform.eulerAngles = rotationAngles;
    }
    public void setCapsulePosition(float[] capsulePosition)
    {
        capsulePlayer = GameObject.Find("Player");
        Vector3 setCapsulePosition;
        setCapsulePosition.x = capsulePosition[0];
        setCapsulePosition.y = capsulePosition[1];
        setCapsulePosition.z = capsulePosition[2];
        capsulePlayer.transform.position = setCapsulePosition;
    }
    public void SaveLevel()
    {
        SaveSystem.SaveLevel(this);
    }
    /* 
    not being currently used but can be used if there's a button
    that needs to fetch saved data
    */
    public void LoadLevel()
    {
        Level2Data data = SaveSystem.LoadLevel();
        setIsCollectedCrsytal(data.isCollectedCrystal);
        setFerryGameObject(data.ferryGameObjectPositionXValue);
        setMakePathGameObject(data.makePathCubePositionYValue);
        setRotateGameObject(data.rotateGameObjectRotationZValue);
        setCapsulePosition(data.capsulePosition);

    }
}
