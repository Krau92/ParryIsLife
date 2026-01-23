using UnityEngine;
using System;
using UnityEditor;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float inmuneTime = 1.0f;
    private int currentHealth;

    public static event Action OnParryStart;
    public static event Action OnParryEnd;
    public static event Action OnReflectingStart;
    public static event Action OnReflectingEnd;
    public static event Action OnDamageTaken;
    public static event Action OnParriedBullet;

    bool inmune;
    bool parrying;
    public bool IsInmune() { return inmune; }
    bool reflecting;

    void Start()
    {
        currentHealth = maxHealth;
        inmune = false;
    }

    //Method to reduce player health
    public void TakeDamage(int damage)
    {
        if (inmune)
        {
            //Player is inmune, no damage taken
            return;
        }

        currentHealth -= damage;

        SetInmune(inmuneTime);

        if (currentHealth <= 0)
        {
            //Handle player death
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
                
                OnParriedBullet?.Invoke();
                //Reflect the bullet
                bullet.ReflectBullet(transform.position);
                return;
            }

            bullet.DeactivateBullet();
            
            if (inmune)
            {
                if(parrying)
                    OnParriedBullet?.Invoke();
                
                return;
            }
            TakeDamage(1);
            
            OnDamageTaken?.Invoke();

        } else if (other.gameObject.CompareTag("Enemy"))
        {
            if (inmune)
            {
                //Player is inmune, no damage taken
                return;
            }

            TakeDamage(1);
            
            OnDamageTaken?.Invoke();
        }


    }

    //Method to set player inmune state
    public void SetInmune(float duration)
    {
        if(inmune)
            CancelInvoke("ResetInmune");
            
        inmune = true;
        Invoke("ResetInmune", duration);

        OnParryStart?.Invoke();
        
    }

    public void SetReflecting(float duration)
    {
        reflecting = true;
        Invoke("ResetReflecting", duration);
        OnReflectingStart?.Invoke();
    }

    private void ResetInmune()
    {
        inmune = false;
        OnParryEnd?.Invoke();
    }

    private void ResetReflecting()
    {
        reflecting = false;
        OnReflectingEnd?.Invoke();
    }

    public void SetParrying(float parryDuration)
    {
        parrying = true;
        SetInmune(parryDuration);
        Invoke("ResetParrying", parryDuration);
    }

    private void ResetParrying()
    {
        parrying = false;
    }
}
