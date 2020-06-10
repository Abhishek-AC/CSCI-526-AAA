
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
Mono Behaviour: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
Base class from which every Unity Script derives
*/

public class ClickToRotate : MonoBehaviour
{
    /*
    GameObject: https://docs.unity3d.com/ScriptReference/GameObject.html
    Base class for all entities in Unity Scenes 
    */
    public GameObject m_rotationCubeGroup;
    public float m_angleSpeed;
    private bool isRotatable;
    private bool isRotating;
    private float maxAnglesPerClick;
    private float currentAngleDegree;
    /* 
    MonoBehaviour.Start() : https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    Start is called to initialize data
    */
    void Start()
    {
        isRotatable = true;
        maxAnglesPerClick = 90f;
        currentAngleDegree = 0f;
    }

    /*
    MonoBehaviour.Update() : https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    Update is called once per frame
    */
    void Update()
    {
        /*
        Input.GetMouseButtonDown() : https://docs.unity3d.com/ScriptReference/Input.GetMouseButtonDown.html
        check if the rotation gear is clicked
        */
        if (Input.GetMouseButtonDown(0) && isRotatable)
        {
            /*
            RaycastHit : https://docs.unity3d.com/ScriptReference/RaycastHit.html
            Property Used - Physics.raycast : https://docs.unity3d.com/ScriptReference/Physics.Raycast.html 
            */
            RaycastHit hit;
            /*
            Ray :https://docs.unity3d.com/ScriptReference/Ray.html
            Camera.main.ScreenPointToRay() : https://docs.unity3d.com/ScriptReference/Camera.ScreenPointToRay.html
            Input.mousePosition : https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
            */
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "RotationGear")
                {
                    isRotating = true;
                    isRotatable = false;
                }
            }
        }
        //if the current status is rotating, rotate the cube group with specified rotating speed
        if (isRotating)
        {
            Debug.Log("m_angleSpeed: " + m_angleSpeed);
            /*
            Vector3 : https://docs.unity3d.com/ScriptReference/Vector3.html
            Vector3 Constructor: https://docs.unity3d.com/ScriptReference/Vector3.html
            */
            Vector3 angleSpeed = new Vector3(0, 0, m_angleSpeed);
            Debug.Log("angleSpeed: " + angleSpeed);

            float frameAngleSpeed = m_angleSpeed * Time.deltaTime;
            Debug.Log("frameAngleSpeed: " + frameAngleSpeed);

            float remainingDegree = maxAnglesPerClick - currentAngleDegree;
            Debug.Log("remainingDegree: " + remainingDegree);

            /*
            check that if the remaing angle( maxAngle - currentAngle) is less than frameAngleSpeed
            Just rotate the remaining angle [Else Part]
            */
            if (remainingDegree < frameAngleSpeed)
            {
                /*
                Transform.rotate : https://docs.unity3d.com/ScriptReference/Transform.Rotate.html
                Space.World : https://docs.unity3d.com/ScriptReference/Space.World.html
                */
                m_rotationCubeGroup.transform.Rotate(new Vector3(0, 0, remainingDegree), Space.World);
                isRotatable = true;
                isRotating = false;
                currentAngleDegree = 0f;
            }
            else
            {
                m_rotationCubeGroup.transform.Rotate(angleSpeed * Time.deltaTime, Space.World);
                currentAngleDegree += frameAngleSpeed;
            }
        }
    }
}