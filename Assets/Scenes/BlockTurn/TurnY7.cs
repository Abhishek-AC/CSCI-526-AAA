using UnityEngine;
using UnityEngine.Events;


// Turn block y7
public class TurnY7 : MonoBehaviour
{
    private bool originalState = true;

    // Add turning event handler
    void Start()
    {
        EventManager.OnClicked += Turn;
    }

    // Decide the turning direction
    void Turn()
    {
        if (originalState)
            TurnBlocksForward();
        else
            TurnBlocksReverse();
    }

    // Original -> New
    private void TurnBlocksForward()
    {
        transform.position = new Vector3(-1f, 7f, 1f);
        originalState = false;
    }

    // New -> Original
    private void TurnBlocksReverse()
    {
        transform.position = new Vector3(0f, 7f, 0f);
        originalState = true;
    }
}
