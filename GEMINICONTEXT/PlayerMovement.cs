using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;

    private Vector2 movementInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0; // Ensure no gravity affects the player
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Freeze rotation
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    //Method to store movement input
    public void SetMoveDirection(Vector2 input)
    {
        movementInput = input;
    }
    
    //Method to move the player based on input
    private void Move()
    {
        // Vector2 newPosition = rb.position + movementInput * speed * Time.fixedDeltaTime;
        // rb.MovePosition(newPosition);

        Vector2 velocity = movementInput * speed;
        rb.linearVelocity = velocity;
    }
}
