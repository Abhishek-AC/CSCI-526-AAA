using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class EndGame : MonoBehaviour
{
    // the path to the save files to be deleted
    private static readonly string[] SAVE_FILES = new string[]
    {
        "level1Data",
        "level2Data"
    };

    // clean any progress and quit the game
    public void Quit()
    {
        Debug.Log("End Game in Build Mode");
        DeleteSaveFiles();
        Application.Quit();
    }

    // clean any progress and restart the game
    public void Replay()
    {
        Debug.Log("Replay");
        DeleteSaveFiles();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

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
