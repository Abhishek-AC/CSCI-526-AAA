using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level2Data
{
    public float [] capsulePosition;
    public bool isCollectedCrystal;
    public float rotateGameObjectRotationZValue;
    public float ferryGameObjectPositionXValue;
    public float makePathCubePositionYValue;

    public Level2Data (Level2 level2 ) {
        capsulePosition = level2.capsulePosition;
        isCollectedCrystal = level2.isCollectedCrystal;
        rotateGameObjectRotationZValue = level2.rotateGameObjectRotationZValue;
        ferryGameObjectPositionXValue = level2.ferryGameObjectPositionXValue;
        makePathCubePositionYValue = level2.makePathCubePositionYValue;
    }

}
