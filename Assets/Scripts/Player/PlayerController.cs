using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

//The "Puppet Master" script that handles player movement and parry mechanics
public class PlayerController : MonoBehaviour
{


    [Header("Player Component References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;

    void OnEnable()
    {
        //Subscribe to input events
        InputManager.onMoveInput += HandleMove;
        InputManager.onParryInput += TryToParry;
    }

    void OnDisable()
    {
        //Unsubscribe from input events
        InputManager.onMoveInput -= HandleMove;
        InputManager.onParryInput -= TryToParry;
    }


    //Method to store movement input
    private void HandleMove(Vector2 input)
    {
        playerMovement.SetMoveDirection(input);
    }

    //Method to attempt parry action
    private void TryToParry()
    {
        playerCombat.TryToParry();
    }

}
