using UnityEngine;

public abstract class LevelManager : MonoBehaviour
{
    // the current state of the level
    public SaveSystem.GameState CurrentState { get; }

    // load level state from file
    public abstract void LoadLevel();

    // save current level state to file
    public abstract void SaveLevel();

    // reset level to initial state
    public abstract void ResetLevel();
}
