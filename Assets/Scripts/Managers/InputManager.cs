using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private Vector2 moveInput;
    public static event Action<Vector2> onMoveInput;
    public static event Action onParryInput;

    public static InputManager Instance { get; private set; }  

    // Awake is called when the script instance is being loaded
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

        playerControls = new PlayerControls();

    }

    // Enable input actions
    void OnEnable()
    {
        playerControls.Enable();
    }


    // Disable input actions
    void OnDisable()
    {
        playerControls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = playerControls.PlayerActions.Move.ReadValue<Vector2>();
        onMoveInput?.Invoke(moveInput);

        //Parry Input Handling
        if (playerControls.PlayerActions.Parry.triggered)
            onParryInput?.Invoke();

        //TODO: Shoot Input Handling (PENDING)
        
    }
}
