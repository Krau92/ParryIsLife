using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float defaultSpeed = 80f;
    [SerializeField] private float slowdownFactor = 0.5f;
    private bool slowed = false;
    private Vector2 playerSize;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera mainCamera;

    Vector3 minScreenBounds, maxScreenBounds;

    private Vector2 movementInput;
    private Rigidbody2D rb;

    void OnEnable()
    {
        slowed = false;
        InputManager.onMoveInput += HandleMoveInput;
    }

    void OnDisable()
    {
        InputManager.onMoveInput -= HandleMoveInput;
    }


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSize = spriteRenderer.bounds.size /2; // Dividido entre 2 para obtener el radio desde el centro

                // 2. Calculamos dónde están los bordes de la pantalla en EL MUNDO
        // (0,0) es la esquina inferior izquierda, (1,1) la superior derecha
        minScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        rb = GetComponent<Rigidbody2D>();
    }


    private void HandleMoveInput(Vector2 input)
    {
        movementInput = input;
        
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        float speed = slowed ? defaultSpeed * slowdownFactor : defaultSpeed;
        rb.linearVelocity = movementInput.normalized * speed;

        // Clamp position to screen bounds
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minScreenBounds.x + playerSize.x, maxScreenBounds.x - playerSize.x),
            Mathf.Clamp(transform.position.y, minScreenBounds.y + playerSize.y, maxScreenBounds.y - playerSize.y),
            transform.position.z
        );
    }

    //Method to set slowdown state
    public void SetSlowed(bool state)
    {
        slowed = state;
    }
    
}
