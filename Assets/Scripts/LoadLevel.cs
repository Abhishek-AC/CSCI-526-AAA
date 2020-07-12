using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadLevel : MonoBehaviour
{
    public void levelToLoad(int level)
    {
        SceneManager.LoadScene(level);
    }
}