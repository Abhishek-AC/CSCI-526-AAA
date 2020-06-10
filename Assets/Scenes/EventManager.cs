using UnityEngine;

// Button for the player to click and trigger block turning
public class EventManager : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction OnClicked;

    // Draw a buttion for the player to click and turn the blocks
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 100, 20, 200, 75), "Click to Turn"))
        {
            if (OnClicked != null)
                OnClicked();
        }
    }
}
