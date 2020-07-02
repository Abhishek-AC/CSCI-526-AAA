using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public void levelToLoad(int level) {
        SceneManager.LoadScene(level);
    }
}
