using UnityEngine;

public class TestSaveDataButton : MonoBehaviour
{
    //! FUNCIONA TODO! TESTEADO Y VERIFICADO.
    [SerializeField] private SaveStateSO saveState;

    public void OnSaveButtonPressed()
    {
        
        ManageSaveData.SaveData(saveState.saveSlot, saveState);
        Debug.Log("Game saved.");
    }

    public void OnLoadButtonPressed()
    {
        ManageSaveData.LoadData(saveState.saveSlot, saveState);
        Debug.Log("Game loaded.");
    }

    public void OnDeleteButtonPressed()
    {
        ManageSaveData.DeleteData(saveState.saveSlot);
        Debug.Log("Save data deleted.");
    }
}
