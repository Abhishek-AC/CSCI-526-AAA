using UnityEngine;

public class RotationManagerLevelThreeDest : MonoBehaviour
{
    public Animator anim;
    private float count = 0;
    private float max_count = 2;
    private bool active = false;
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            count += Time.deltaTime;
            if (count >= max_count)
            {
                StopAnimation();
            }
        }
        //the following code connects cubes after rotation has been made
        Transform cubeToConnect1 = transform.Find("Cube (71)");
        Transform cubeToConnect2 = GameObject.Find("Cube (50)").transform;
        Transform cubeToConnect3 = GameObject.Find("Cube (48)").transform;
        //return if Transform not found
        if (cubeToConnect1 == null || cubeToConnect2 == null)
        {
            Debug.Log("Cube not reachable");
            return;
        }
        //check if the rotation angle is 90 degree, which connects two blocks
        if (Mathf.Abs(transform.rotation.eulerAngles.x - 90f) < 0.1f)
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[1].active = true;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = true;
            cubeToConnect3.GetComponent<Walkable>().possiblePath[2].active = false;
            cubeToConnect1.GetComponent<Walkable>().possiblePath[2].active = false;
            cubeToConnect1.GetComponent<Walkable>().canWalkOnThisBlock = true;
        }
        else
        {
            cubeToConnect1.GetComponent<Walkable>().possiblePath[1].active = false;
            cubeToConnect2.GetComponent<Walkable>().possiblePath[1].active = false;
            cubeToConnect1.GetComponent<Walkable>().canWalkOnThisBlock = false;
        }
    }
    public void StopAnimation()
    {
        Destroy(anim);
    }
    public void ActivateAnimation()
    {
        active = true;
        anim.SetBool("active", true);
    }
}
