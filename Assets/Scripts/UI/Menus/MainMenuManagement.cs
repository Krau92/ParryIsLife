using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuManagement : MonoBehaviour
{
    GameObject lastButtonSelected;

    void OnEnable()
    {
        GameEvents.OnButtonPressed += UpdateLastButtonSelected;
        GameEvents.OnPanelClosed += SetLastButtonSelected;
    }

    void OnDisable()
    {
        GameEvents.OnButtonPressed -= UpdateLastButtonSelected;
        GameEvents.OnPanelClosed -= SetLastButtonSelected;
    }

    void UpdateLastButtonSelected(GameObject button)
    {
        lastButtonSelected = button;
    }

    public void SetLastButtonSelected()
    {
        if (lastButtonSelected != null)
        {
            SetNextButtonSelected(lastButtonSelected);
        }
    }

    public void SetNextButtonSelected(GameObject button)
    {
        EventSystem.current.SetSelectedGameObject(button);
    }
}
