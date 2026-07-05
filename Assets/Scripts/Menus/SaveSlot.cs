using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    public int slotIndex; // Index of the save slot
    int totalThreatLevel; // Total threat level for this save slot
    int totalBossesDefeated; // Total bosses defeated for this save slot
    bool thereIsSavedData = false; // Flag to check if data exists
    public SaveStateSO saveState; // Reference to the SaveStateSO ScriptableObject
    public TMP_Text dataText; // Reference to the Text component to display data
    public GameObject deleteButton; // Reference to the delete button GameObject


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckForSavedData();
    }

    void CheckForSavedData()
    {
        thereIsSavedData = ManageSaveData.GetFileData(slotIndex, out totalThreatLevel, out totalBossesDefeated);
        string text = "SLOT #" + (slotIndex + 1) + "\n";
        text += "Total Threat Level: " + totalThreatLevel + "\n";
        text += "Total Bosses Defeated: " + totalBossesDefeated;
        dataText.text = thereIsSavedData ? text : "SLOT #" + (slotIndex + 1) + "\nNo Data";

        if(thereIsSavedData)
        {
            deleteButton.SetActive(true);
        }
        else
        {
            deleteButton.SetActive(false);
        }
    }

    public void OnClickLoad()
    {
        if (thereIsSavedData)
        {
            // Load the game using the save data for this slot
            ManageSaveData.LoadData(slotIndex, saveState);
        }
        else
        {
            saveState.ResetSaveState(slotIndex);
        }
    }

    public void OnClickDelete()
    {
        if (thereIsSavedData)
        {
            MenuUtils.Instance.DelayedFunction(DeleteSaveData, 0.5f, "Are you sure you want to delete this save data?\nPress Submit to confirm or Cancel to abort.");
                
            CheckForSavedData(); // Refresh the display after deletion
        }
    }

    public void DeleteSaveData()
    {
        ManageSaveData.DeleteData(slotIndex);
        thereIsSavedData = false;
        CheckForSavedData(); // Refresh the display after deletion
    }

}
