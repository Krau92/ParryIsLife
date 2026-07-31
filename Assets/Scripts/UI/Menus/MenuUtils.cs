using System;
using System.Collections;
using UnityEngine;

public class MenuUtils : MonoBehaviour
{
    public TextBox textBox;
    bool readyToExecute = false;
    bool submitPressed = false;
    bool cancelPressed = false;
    Coroutine currentCoroutine;

    public static MenuUtils Instance { get; private set; }

    void Awake()
    {
        //Singleton Pattern Implementation
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        InputManager.onSubmitInput += HandleSubmitInput;
        InputManager.onCancelInput += HandleCancelInput;
    }

    void OnDisable()
    {
        InputManager.onSubmitInput -= HandleSubmitInput;
        InputManager.onCancelInput -= HandleCancelInput;
    }

    public void DelayedFunction(Action action, float delay, string textToShow)
    {
        if(currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(DelayedFunctionCoroutine(action, delay, textToShow));
        }
    }

    private IEnumerator DelayedFunctionCoroutine(Action action, float delay, string textToShow)
    {
        // Show the text to the user
        ShowText(textToShow);

        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        readyToExecute = true;

        yield return new WaitUntil(() => submitPressed || cancelPressed);
        readyToExecute = false;

        // Execute the action
        if (submitPressed)
        {
            action?.Invoke();
        }

        submitPressed = false;
        cancelPressed = false;

        HideText();
        currentCoroutine = null;
    }

    private void ShowText(string text)
    {
        textBox.SetText(text);
        textBox.SetTextBoxVisibility(true);
    }

    private void HideText()
    {
        textBox.SetTextBoxVisibility(false);
    }

    private void HandleSubmitInput()
    {
        if (readyToExecute)
        {
            submitPressed = true;
        }
    }

    private void HandleCancelInput()
    {
        if (readyToExecute)
        {
            cancelPressed = true;
        }
    }

    

}
