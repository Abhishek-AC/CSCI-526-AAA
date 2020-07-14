using UnityEngine;

/* 
Mono Behaviour: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
Base class from which every Unity Script derives
*/

public class ClickToRotateLevelThreeKey : MonoBehaviour
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

    private bool IsPlayerOnRotate
    {
        get
        {
            var result = false;
            var rotate = GameObject.Find("Rotate_Key");
            var player = GameObject.Find("Player");

            if (Physics.Raycast(player.transform.position, -player.transform.up, out var playerHit))
                if (playerHit.transform.IsChildOf(rotate.transform))
                    result = true;

            return result;
        }
    }

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
        //if the current status is rotating, rotate the cube group with specified rotating speed
        if (isRotating)
        {
            /*
            Vector3 : https://docs.unity3d.com/ScriptReference/Vector3.html
            Vector3 Constructor: https://docs.unity3d.com/ScriptReference/Vector3.html
            */
            Vector3 angleSpeed = new Vector3(0, 0, m_angleSpeed);
            float frameAngleSpeed = m_angleSpeed * Time.deltaTime;
            float remainingDegree = maxAnglesPerClick - currentAngleDegree;

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
            //kill any player movement
            GameObject player = GameObject.Find("Player");
            player.transform.GetComponent<PlayerController>().KillMovement();
        }
    }
    
    void OnMouseDown()
    {
        if (isRotatable && !IsPlayerOnRotate)
        {
            transform.GetComponent<AudioSource>().Play();  //SFX
            isRotating = true;
            isRotatable = false;
        }
    }
}
