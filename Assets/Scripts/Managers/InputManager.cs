using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private Vector2 moveInput;
    public static event Action<Vector2> onMoveInput;

    public static event Action onParryInput;
    public static event Action onStopParryInput;

    public static event Action onShootInput;
    public static event Action onStopShootInput;

    public static event Action onMeleeInput;

    public static InputManager Instance { get; private set; }  
    

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        //Singleton Pattern Implementation
        if (Instance == null)
        {
            Instance = this;
            
            // Configure frame rate and physics timestep
            Application.targetFrameRate = 60;
            Time.fixedDeltaTime = 0.016667f; // 60 Hz physics (1/60)
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

        if (playerControls.PlayerActions.Parry.WasReleasedThisFrame())
            onStopParryInput?.Invoke();


        //Shoot Input Handling
        if (playerControls.PlayerActions.Shoot.triggered)
            onShootInput?.Invoke();

        if (playerControls.PlayerActions.Shoot.WasReleasedThisFrame())
            onStopShootInput?.Invoke();

        //Melee Input Handling
        if (playerControls.PlayerActions.Melee.triggered)
            onMeleeInput?.Invoke();
    }

    public bool IsParryHold()
    {
        return playerControls.PlayerActions.Parry.IsPressed();
    }

    public bool IsShootHold()
    {
        return playerControls.PlayerActions.Shoot.IsPressed();
    }
}
