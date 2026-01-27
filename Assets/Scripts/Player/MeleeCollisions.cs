using UnityEngine;

public class MeleeCollisions : MonoBehaviour
{
    [SerializeField] private PlayerMelee parent;


    void OnTriggerEnter2D(Collider2D other)
    {
        parent.HandleCollision(other.gameObject);
    }
}
