using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    public int MaxHealth { get { return maxHealth; } }

    [SerializeField] private float inmuneTime = 1.0f;

    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    bool inmune;
    bool parrying;
    public bool IsInmune() { return inmune; }
    bool reflecting;

    void Start()
    {
        ResetPlayerHealth();
    }

    //Method to reset player health
    public void ResetPlayerHealth()
    {
        currentHealth = maxHealth;
        inmune = false;
        parrying = false;
        reflecting = false;
    }

    //Method to reduce player health
    public void TakeDamage(int damage)
    {
        //Security check 
        if(currentHealth <= 0)
            return; 


        if (inmune)
        {
            //Player is inmune, no damage taken
            return;
        }

        currentHealth -= damage;
        CombatEvents.OnDamageTaken?.Invoke();

        SetInmune(inmuneTime);

        if (currentHealth <= 0)
        {
            //!Debería ir una animación que triggerea el método de InvokeDeathEvent en acabar
            InvokeDeathEvent();
        }
    }

    //Handle collisions
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            NewTestBullet bullet = other.gameObject.GetComponent<NewTestBullet>();
            if (reflecting)
            {
                
                CombatEvents.OnParriedBullet?.Invoke();
                //Reflect the bullet
                bullet.ReflectBullet(transform.position);
                return;
            }

            
            if (inmune)
            {
                if(parrying)
                {
                    CombatEvents.OnParriedBullet?.Invoke();
                    bullet.ParriedBullet(transform.position);
                }
                
                return;
            }
            
            bullet.DeactivateBullet();
            TakeDamage(1);

        } else if (other.gameObject.CompareTag("Enemy"))
        {
            if (inmune)
            {
                //Player is inmune, no damage taken
                return;
            }

            TakeDamage(1);
            
            CombatEvents.OnDamageTaken?.Invoke();
        }


    }

    //Method to set player inmune state
    public void SetInmune(float duration)
    {
        if(inmune)
            CancelInvoke("ResetInmune");
            
        inmune = true;
        Invoke("ResetInmune", duration);
        
    }

    public void SetReflecting(float duration)
    {
        reflecting = true;
        Invoke("ResetReflecting", duration);
        CombatEvents.OnReflectingStart?.Invoke();
    }

    private void ResetInmune()
    {
        inmune = false;
    }

    private void ResetReflecting()
    {
        reflecting = false;
        CombatEvents.OnReflectingEnd?.Invoke();
    }

    public void SetParrying(float parryDuration)
    {
        parrying = true;
        SetInmune(parryDuration);
        //Set reflecting
        // SetReflecting(parryDuration);
        Invoke("ResetParrying", parryDuration);
        CombatEvents.OnParryStart?.Invoke();
    }

    private void ResetParrying()
    {
        parrying = false;
        CombatEvents.OnParryEnd?.Invoke();
    }

    private void InvokeDeathEvent()
    {
        CombatEvents.OnPlayerDeath?.Invoke();
    }
}
