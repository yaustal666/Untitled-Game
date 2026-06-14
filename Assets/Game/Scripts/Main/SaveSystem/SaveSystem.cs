using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : ISaveRegistry
{
    public bool HasSaveFile => File.Exists(savePath);

    private string savePath;
    private List<ISavable> listSave = new();

    public SaveSystem()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }

    public void Register(ISavable item)
    {
        listSave.Add(item);
    }

    public void SaveGame()
    {
        GameSaveData saveData = new GameSaveData();

        foreach (var item in listSave)
        {
            item.Save(saveData);
        }

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(savePath, json);

        Debug.Log("Game saved successfully!");
    }

    public void LoadGame()
    {
        if (!HasSaveFile)
        {
            CreateEmptySave();
        }

        string json = File.ReadAllText(savePath);
        GameSaveData saveData = JsonConvert.DeserializeObject<GameSaveData>(json);

        foreach (var item in listSave)
        {
            item.Load(saveData);
        }
    }

    private void CreateEmptySave()
    {
        GameSaveData saveData = new();
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);

        using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(json);
            }
        }
    }

    public void ClearSaveData()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("Save data cleared");
        }
    }
}