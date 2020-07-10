using UnityEngine;

public class MoveManager : MonoBehaviour
{
    // Update is called once per frame
    private float maxRange = -3;
    private float minRange = -5;
    public GameObject MoveGroup1_1;
    public GameObject MoveGroup1_2;
    public GameObject MoveGroup2_1;
    public GameObject MoveGroup2_2;
    public Animator anim;
    private float timeCount = 0;
    private float AnimStopTime = 2;
    void Update()
    {
        //stop animation after some time
        timeCount += Time.deltaTime;
        if(timeCount >= AnimStopTime)
        {
            StopAnimation();
        }
        //check whether the moving block gourp reaches either end, if so change the possible path of related blocks to make movement possible
        if (transform.position.x == maxRange)
        {
            MoveGroup1_1.transform.GetComponent<Walkable>().possiblePath[0].active = true;
            MoveGroup1_2.transform.GetComponent<Walkable>().possiblePath[0].active = true;
            MoveGroup2_1.transform.GetComponent<Walkable>().possiblePath[0].active = false;
            MoveGroup2_2.transform.GetComponent<Walkable>().possiblePath[0].active = false;
        }
        else if (transform.position.x == minRange)
        {
            MoveGroup1_1.transform.GetComponent<Walkable>().possiblePath[0].active = false;
            MoveGroup1_2.transform.GetComponent<Walkable>().possiblePath[0].active = false;
            MoveGroup2_1.transform.GetComponent<Walkable>().possiblePath[0].active = true;
            MoveGroup2_2.transform.GetComponent<Walkable>().possiblePath[0].active = true;
        }
        else
        {
            MoveGroup1_1.transform.GetComponent<Walkable>().possiblePath[0].active = false;
            MoveGroup1_2.transform.GetComponent<Walkable>().possiblePath[0].active = false;
            MoveGroup2_1.transform.GetComponent<Walkable>().possiblePath[0].active = false;
            MoveGroup2_2.transform.GetComponent<Walkable>().possiblePath[0].active = false;
        }
    }
    public void StopAnimation()
    {
        Destroy(anim);
    }
}
