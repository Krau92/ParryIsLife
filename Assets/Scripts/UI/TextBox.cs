using UnityEngine;
using TMPro;

public class TextBox : MonoBehaviour
{
    public TMP_Text textComponent;

    void Awake()
    {
        SetTextBoxVisibility(false); // Hide the text box initially
    }

    public void SetText(string text)
    {
        if (textComponent != null)
        {
            textComponent.text = text;
        }
    }

    public void SetTextBoxVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
