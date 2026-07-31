using UnityEngine;

public class ButtonEventCaller : MonoBehaviour
{
    public void CallButtonPressedEvent()
    {
        GameEvents.OnButtonPressed?.Invoke(gameObject);
    }
}
