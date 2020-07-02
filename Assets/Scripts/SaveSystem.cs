using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{
    public static void SaveLevel(Level2 level2)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/level2Data";
        FileStream stream = new FileStream(path, FileMode.Create);

        Level2Data data = new Level2Data(level2);

        formatter.Serialize(stream, data);
        Debug.Log("Data Saved");
        stream.Close();
    }
    public static Level2Data LoadLevel()
    {
        string path = Application.persistentDataPath + "/level2Data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Level2Data data = formatter.Deserialize(stream) as Level2Data;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save File Not Found in " + path);
            return null;
        }
    }
}
