using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; //SFX
public class LoadLevel : MonoBehaviour
{
    public void levelToLoad(int level)
    {
        // SceneManager.LoadScene(level);
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(LoadNewScene(level));
    }

    //SFX++
    IEnumerator LoadNewScene(int level)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(level);
    }
    //SFX--
}