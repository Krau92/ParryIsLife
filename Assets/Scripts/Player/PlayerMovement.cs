using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    private Vector2 playerSize;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera mainCamera;

    private Vector2 movementInput;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSize = spriteRenderer.bounds.size /2; // Dividido entre 2 para obtener el radio desde el centro
    }

    void Update()
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
        // 1. Calculamos la posición futura deseada
        Vector3 nextPosition = transform.position + (Vector3)(movementInput * speed * Time.deltaTime);

        // 2. Calculamos dónde están los bordes de la pantalla en EL MUNDO
        // (0,0) es la esquina inferior izquierda, (1,1) la superior derecha
        Vector3 minScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 maxScreenBounds = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // 3. Aplicamos el CLAMP (La restricción)
        // La lógica es: No puedes pasar de (BordeIzquierdo + MiAncho) ni de (BordeDerecho - MiAncho)
        
        // Eje X
        nextPosition.x = Mathf.Clamp(
            nextPosition.x, 
            minScreenBounds.x + playerSize.x, 
            maxScreenBounds.x - playerSize.x
        );

        // Eje Y
        nextPosition.y = Mathf.Clamp(
            nextPosition.y, 
            minScreenBounds.y + playerSize.y, 
            maxScreenBounds.y - playerSize.y
        );

        // 4. Aplicamos la posición final corregida
        transform.position = nextPosition;
    }
}
