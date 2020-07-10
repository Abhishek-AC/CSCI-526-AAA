using UnityEngine;

public class RotationManager : MonoBehaviour
{
    public Animator anim;
    private float timeCount = 0;
    private float AnimStopTime = 2;
    private bool active = false;
    // Update is called once per frame
    void Update()
    {
        //stop animation after a certain period of time
        if (active)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= AnimStopTime)
            {
                StopAnimation();
                active = false;
            }
        }

        //the following code connects cubes after rotation has been made
        Transform cubeToConnect1 = transform.Find("Cube (y4)");
        Transform cubeToConnect2 = GameObject.Find("Cube (x-5y8z1)").transform;
        //return if Transform not found
        if (cubeToConnect1 == null || cubeToConnect2 == null)
        {
            Debug.Log("Cube not reachable");
            return;
        }
        //check if the rotation angle is 270 degree, which connects two blocks
        if (Mathf.Abs(transform.rotation.eulerAngles.z - 270f) < 0.1f)
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[0].active = true;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = true;
            cubeToConnect2.GetComponent<Walkable>().canWalkOnThisBlock = true;
        }
        else
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[0].active = false;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = false;
            cubeToConnect2.GetComponent<Walkable>().canWalkOnThisBlock = false;
        }
    }
    public void StopAnimation()
    {
        Destroy(anim);
    }
    public void ActivateAnimation()
    {
        anim.SetBool("active", true);
        active = true;
    }
}
