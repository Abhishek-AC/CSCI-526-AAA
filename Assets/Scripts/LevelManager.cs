using UnityEngine;

public abstract class LevelManager : MonoBehaviour
{
    // load level state from file
    public abstract void LoadLevel();

    // save current level state to file
    public abstract void SaveLevel();

    // reset level to initial state
    public abstract void ResetLevel();
}
