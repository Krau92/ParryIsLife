using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;


    private PlayerCombat playerCombat;

    void Start()
    {
        currentHealth = maxHealth;
        playerCombat = GetComponent<PlayerCombat>();
    }

    //Method to reduce player health
    public void TakeDamage(int damage)
    {
        if (playerCombat != null && playerCombat.IsParrying)
        {
            //Player is parrying, no damage taken
            return;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            //Handle player death
        }
    }

    //Handle collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Bullet"))
        {
            if (playerCombat != null && playerCombat.IsParrying)
            {
                //Player is parrying, no damage taken
                return;
            }
            TakeDamage(1);
        }


    }
}
