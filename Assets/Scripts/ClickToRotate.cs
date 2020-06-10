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
    // Start is called before the first frame update
    void Start()
    {
        isRotatable = true;
        maxAnglesPerClick = 90f;
        currentAngleDegree = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isRotatable)
        {
            isRotating = true;
            isRotatable = false;
        }
        if (isRotating)
        {
            Vector3 angleSpeed = new Vector3(0, 0, m_angleSpeed);
            currentAngleDegree += m_angleSpeed*Time.deltaTime;
            m_rotationCubeGroup.transform.Rotate(angleSpeed*Time.deltaTime, Space.World);
            if (currentAngleDegree >= maxAnglesPerClick)
            {
                isRotatable = true;
                isRotating = false;
                currentAngleDegree = 0f;
            }
        }
    }
}
