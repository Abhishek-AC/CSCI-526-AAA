using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToRotate : MonoBehaviour
{
    public GameObject m_rotationCubeGroup;
    public float m_angleSpeed;
    private bool isRotatable;
    private bool isRotating;
    private float maxAnglesPerClick;
    private float currentAngleDegree;
    // Start is called to initialize data
    void Start()
    {
        isRotatable = true;
        maxAnglesPerClick = 90f;
        currentAngleDegree = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the rotation gear is clicked
        if (Input.GetMouseButtonDown(0) && isRotatable)
        {
            RaycastHit hit;
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
            Vector3 angleSpeed = new Vector3(0, 0, m_angleSpeed);
            float frameAngleSpeed =  m_angleSpeed*Time.deltaTime;
            float remainingDegree = maxAnglesPerClick - currentAngleDegree;
            //check that if the remaing angle( maxAngle - currentAngle) is less than frameAngleSpeed,just rotate the remaining angle
            if ( remainingDegree < frameAngleSpeed)
            {
                m_rotationCubeGroup.transform.Rotate(new Vector3(0,0,remainingDegree), Space.World);
                isRotatable = true;
                isRotating = false;
                currentAngleDegree = 0f;
            }
            else {
                m_rotationCubeGroup.transform.Rotate(angleSpeed*Time.deltaTime, Space.World);
                currentAngleDegree += frameAngleSpeed;
            }
        }
    }
}
