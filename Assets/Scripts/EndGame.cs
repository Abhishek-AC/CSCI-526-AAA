using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
public class EndGame : MonoBehaviour
{
    public void Quit()
    {
        Debug.Log("End Game in Build Mode");
        DeleteFile();
        Application.Quit();
    }
    public void Relay()
    {
        Debug.Log("Replay");
        DeleteFile();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    void DeleteFile()
    {
        string path = Application.persistentDataPath + "/level2Data";
        // check if file exists
        if (!File.Exists(path))
        {
            Debug.Log("Save File Not Found in " + path);
        }
        else
        {
            Debug.Log("Deleting the state");
            File.Delete(path);
            RefreshEditorProjectWindow();
        }
    }
    void RefreshEditorProjectWindow()
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
