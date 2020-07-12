using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; //SFX

public class PauseMenu : MonoBehaviour
{
    public GameObject Pause_Menu, PauseButton, Level, InfoButton, Shaft_with_spokes;

    public void Pause()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(Wait());

        Level.SetActive(false);
        Shaft_with_spokes.SetActive(false);
        InfoButton.SetActive(false);
        Pause_Menu.SetActive(true);
        PauseButton.SetActive(false);

    }

    //SFX++
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        
    }
    //SFX--

    public void Resume()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(Wait());

        Level.SetActive(true);
        Shaft_with_spokes.SetActive(true);
        InfoButton.SetActive(true);
        Pause_Menu.SetActive(false);
        PauseButton.SetActive(true);
    }

    public void Quit()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(WaitandQuit());

        /*Debug.Log("Game End in Build Mode");
        var levelManager = Level.GetComponent<LevelManager>();
        if (levelManager != null) levelManager.ResetLevel();
        Application.Quit();*/
    }

    //SFX++
    IEnumerator WaitandQuit()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Game End in Build Mode");
        var levelManager = Level.GetComponent<LevelManager>();
        if (levelManager != null) levelManager.ResetLevel();
        Application.Quit();

    }
    //SFX--

    public void Replay()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(WaitandReplay());

       /* Debug.Log("Replay");
        var levelManager = Level.GetComponent<LevelManager>();
        if (levelManager != null) levelManager.ResetLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);*/
    }

    //SFX++
    IEnumerator WaitandReplay()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Replay");
        var levelManager = Level.GetComponent<LevelManager>();
        if (levelManager != null) levelManager.ResetLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    //SFX--

    public void Info()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(LoadNewScene());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    //SFX++
    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
    //SFX--

}
