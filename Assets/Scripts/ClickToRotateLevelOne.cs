using UnityEngine;

public class ClickToRotateLevelOne : MonoBehaviour
{
    private bool toRotate;
    private float rotationAngle = 140f;
    private float rotationSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        //bool value to check if are allowed to rotate the level
        toRotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "RotationGear2")
                {
                    //if we click on RotationGear2, we allow the level to be rotated
                    toRotate = true;
                }
            }
        }

        if (toRotate)
        {
            var level = GameObject.Find("Level1");
            rotationAngle = level.transform.rotation.eulerAngles.y;
            //stop the rotation when y rotation equals 0
            if (rotationAngle < 0.1f)
                rotationSpeed = 0;
            else
            {
                //kill any player movement
                var player = GameObject.Find("Player");
                player.transform.GetComponent<PlayerController>().KillMovement();
            }
            level.transform.Rotate(0, rotationSpeed, 0);
        }
    }
}
