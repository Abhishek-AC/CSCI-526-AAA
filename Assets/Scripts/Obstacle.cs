using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // the target position of the obstacle object once the collectable is destroyed
    private static readonly Vector3 TARGET = new Vector3(-1f, 7f, 6f);

    // Update is called once per frame
    void Update()
    {
        // the walkable component of the obstacle object
        var walkable = transform.GetComponent<Walkable>();

        // find the collectable object
        var collectable = GameObject.FindWithTag("crystal");

        // move the obstacle object to the target position once the collectable is destroyed
        if (collectable == null && transform.position != TARGET)
            transform.position = Vector3.MoveTowards(transform.position, TARGET, Time.deltaTime);

        // activate the obstacle block only if it is at the right position
        if (transform.position == TARGET)
        {
            walkable.possiblePath[0].active = true;
            walkable.possiblePath[1].active = true;
            walkable.canWalkOnThisBlock = true;
        }
        else
        {
            walkable.possiblePath[0].active = false;
            walkable.possiblePath[1].active = false;
            walkable.canWalkOnThisBlock = false;
        }
    }
}
