using UnityEngine;
using System.IO;

public static class ManageSaveData
{
    public static void SaveData(int slotIndex, SaveStateSO saveData)
    {
        string jsonData = JsonUtility.ToJson(saveData, true);
        string filePath = Application.persistentDataPath + $"/save_slot_{slotIndex}.json";

        File.WriteAllText(filePath, jsonData);

        Debug.Log($"Game saved to {filePath}");
    }

    public static void LoadData(int slotIndex, SaveStateSO saveData)
    {
        string filePath = Application.persistentDataPath + $"/save_slot_{slotIndex}.json";

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(jsonData, saveData);
        }
        else
        {
            Debug.LogWarning($"Save file not found at {filePath}");
        }
    }

    public static void DeleteData(int slotIndex)
    {
        string filePath = Application.persistentDataPath + $"/save_slot_{slotIndex}.json";

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            Debug.LogWarning($"Save file not found at {filePath}");
        }
    }
    
}
