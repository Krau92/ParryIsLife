using UnityEngine;

public class MeleeCollisions : MonoBehaviour
{
    private PlayerMelee parent;

    void Start()
    {
        parent = GetComponentInParent<PlayerMelee>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        parent.HandleCollision(other.gameObject);
        Debug.Log("Melee hit: " + other.gameObject.name);
    }
}
