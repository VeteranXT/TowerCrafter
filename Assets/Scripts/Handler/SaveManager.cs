using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string SaveDirectory = Application.persistentDataPath;
    public static void SaveData<T>(T data, string saveName, string directory )
    {
        var fullpath = Path.Combine(SaveDirectory, directory);
        if (!Directory.Exists(fullpath))
        {
            Directory.CreateDirectory(fullpath);
        }
        // Create full filePath path
        string filePath = Path.Combine(fullpath, saveName);
        // Convert data to JSON
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"[Save Manager] saved data to {filePath}");
    }

    public static T LoadData<T>(string SaveFile, string directory)
    {
       ;
        string filePath = Path.Combine(SaveDirectory, directory, SaveFile);
        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"[SaveManager] File not found: {filePath}");
            return default;

        }
        string json = File.ReadAllText(filePath);
        T playerData = JsonUtility.FromJson<T>(json);
        return playerData;
    }
}




