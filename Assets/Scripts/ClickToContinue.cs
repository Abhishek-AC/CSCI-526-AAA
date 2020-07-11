using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToContinue : MonoBehaviour
{
    public void ContiueGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(LoadNewScene());
    }

    //SFX++
    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    //SFX--
}
