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
        //if the current status is rotating, rotate the cube group in specified rotating speed
        if (isRotating)
        {
            Vector3 angleSpeed = new Vector3(0, 0, m_angleSpeed);
            currentAngleDegree += m_angleSpeed*Time.deltaTime;
            //only rotate when the angle that has been rotated is less than the maximum rotating degree per click
            if(currentAngleDegree < maxAnglesPerClick)
            {
                m_rotationCubeGroup.transform.Rotate(angleSpeed * Time.deltaTime, Space.World);
                float remaingDegree = maxAnglesPerClick - currentAngleDegree;
                if (remaingDegree <= m_angleSpeed * Time.deltaTime)
                {
                    m_rotationCubeGroup.transform.Rotate(new Vector3(0,0,remaingDegree), Space.World);
                }
            }
            else { 
                isRotatable = true;
                isRotating = false;
                currentAngleDegree = 0f;
            }
        }
    }
}
