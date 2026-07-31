using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPanel : MonoBehaviour
{
    
    public GameObject defaultSelectedButton;

    protected void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ToggleMenu()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
        } else
        {
            GameEvents.OnPanelClosed?.Invoke();
        }
    }
}
