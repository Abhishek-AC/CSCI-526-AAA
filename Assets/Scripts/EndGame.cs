using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections; //SFX
public class EndGame : MonoBehaviour
{
    // the path to the save files to be deleted
    private static readonly string[] SAVE_FILES = new string[]
    {
        "level1Data",
        "level2Data",
        "level3Data"
    };

    // clean any progress and quit the game
    public void Quit()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(WaitandQuit());
        /*Debug.Log("End Game in Build Mode");
        DeleteSaveFiles();
        Application.Quit();*/
    }

    //SFX++
    IEnumerator WaitandQuit()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("End Game in Build Mode");
        DeleteSaveFiles();
        Application.Quit();

    }
    //SFX--

    // clean any progress and restart the game
    public void Replay()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Play();
        StartCoroutine(WaitandReplay());
       /* Debug.Log("Replay");
        DeleteSaveFiles();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);*/
    }

    //SFX++
    IEnumerator WaitandReplay()
    {
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Replay");
        DeleteSaveFiles();
        // re-directing to level select screen
        SceneManager.LoadScene(1);

    }
    //SFX--

    // delete the save files
    void DeleteSaveFiles()
    {
        foreach (var f in SAVE_FILES)
            try
            {
                File.Delete(Path.Combine(Application.persistentDataPath, f));
                Debug.Log($"Successfully deleted save file {f}");
            }
            // in case deletion failed
            catch (IOException ex)
            {
                Debug.Log($"Delete save file failed: {ex.Message}");
            }
    }

    void RefreshEditorProjectWindow()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
