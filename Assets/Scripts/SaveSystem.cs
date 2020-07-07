using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// manages the serialization and deserialization of game state
public static class SaveSystem
{
    // a utility class that serves as the base class for the game state of different levels
    // all game state classes must inherit from this class in order to be serialized correctly
    // currently this is used to prevent one feeding garbage to the game state serializer
    // but if we need, we can put utilty methods here
    [Serializable]
    public abstract class GameState { }

    // name of the level 1 save file
    private static readonly string LEVEL_ONE_SAVE_FILE = "level1Data";
    // name of the level 2 save file
    private static readonly string LEVEL_TWO_SAVE_FILE = "level2Data";
    // name of the level 3 save file
    private static readonly string LEVEL_THREE_SAVE_FILE = "level3Data";

    // save level 1 state to file
    public static void SaveLevelOne(LevelOne level) => Save(level.CurrentState,
        Path.Combine(Application.persistentDataPath, LEVEL_ONE_SAVE_FILE));

    // save level 2 state to file
    public static void SaveLevelTwo(LevelTwo level) => Save(level.CurrentState,
        Path.Combine(Application.persistentDataPath, LEVEL_TWO_SAVE_FILE));

    // save level 3 state to file
    public static void SaveLevelThree(LevelThree level) => Save(level.CurrentState,
        Path.Combine(Application.persistentDataPath, LEVEL_THREE_SAVE_FILE));

    // read level 1 state from save file
    public static LevelOne.LevelOneState LoadLevelOne() => Load<LevelOne.LevelOneState>(
        Path.Combine(Application.persistentDataPath, LEVEL_ONE_SAVE_FILE));

    // read level 2 state from save file
    public static LevelTwo.LevelTwoState LoadLevelTwo() => Load<LevelTwo.LevelTwoState>(
        Path.Combine(Application.persistentDataPath, LEVEL_TWO_SAVE_FILE));

    // read level 3 state from save file
    public static LevelThree.LevelThreeState LoadLevelThree() => Load<LevelThree.LevelThreeState>(
        Path.Combine(Application.persistentDataPath, LEVEL_THREE_SAVE_FILE));

    // generic logic for saving game state to file
    private static void Save<T>(T data, string path) where T : GameState
    {
        // make sure that we don't have an empty path
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException(
                "Save file path cannot be empty", nameof(path));

        try
        {
            var formatter = new BinaryFormatter();
            // note: Visual Studio may report tha the "using" statement can be simplified
            // disregard it since it is a new language syntax that Unity does not yet support
            using (var stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, data);
                Debug.Log($"Level saved to file {path}");
            }

        }
        // in case file I/O failed
        catch (IOException ex)
        {
            Debug.Log($"Cannot save level to file: {ex.Message}");
        }
        // in case the serializer failed for some reason
        catch (SerializationException ex)
        {
            Debug.Log($"Write level save file failed: {ex.Message}");
        }
    }

    // generic logic for reading game state from save file
    private static T Load<T>(string path) where T : GameState
    {
        // make sure that we don't have an empty path
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException(
                "Save file path cannot be empty", nameof(path));

        try
        {
            var formatter = new BinaryFormatter();
            // note: Visual Studio may report tha the "using" statement can be simplified
            // disregard it since it is a new language syntax that Unity does not yet support
            using (var stream = new FileStream(path, FileMode.Open))
            {
                var data = (T)formatter.Deserialize(stream);
                return data;
            }
        }
        // in case file I/O failed
        catch (IOException ex)
        {
            Debug.Log($"Cannot read save file (expected in a fresh game): {ex.Message}");
            return default;
        }
        // in case the save file itself is corrupted
        catch (SerializationException ex)
        {
            Debug.Log($"Read from save file failed (possibly corrupted): {ex.Message}");
            return default;
        }
    }
}
