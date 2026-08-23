using System.IO;
using UnityEngine;

public class SaveService
{
    private const string FileName = "save_data.json";

    public static SaveService Instance { get; } = new();

    public SaveData Data { get; private set; }

    private string FilePath => 
        Path.Combine(Application.persistentDataPath, FileName);

    public void Save()
    {
        if(Data == null)
            Data = new SaveData();

        string json = JsonUtility.ToJson(Data, true);

        File.WriteAllText(FilePath, json);
    }

    public void Load()
    {
        if(!File.Exists(FilePath))
        {
            Data = new SaveData();
            return;
        }

        string json = File.ReadAllText(FilePath);

        Data = JsonUtility.FromJson<SaveData>(json);

        if (Data == null)
            Data = new SaveData();
    }

    public bool CompleteLevel(int levelId)
    {
        if (Data.Levels.CompletedLevels.Contains(levelId))
            return false;

        Data.Levels.CompletedLevels.Add(levelId);

        return true;
    }
}

