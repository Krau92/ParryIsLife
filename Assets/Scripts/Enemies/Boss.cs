using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossPhaseData
{
    public string phaseName; //To organize and make easier to identify
    public float healthThreshold;
}

public abstract class Boss : MonoBehaviour
{
    [SerializeField] protected BossAnimationManaging animationManager;
    [SerializeField] protected int maxHealth;
    public int MaxHealth { get { return maxHealth; } }
    [SerializeField] protected List<BossPhaseData> phasesData;
    [SerializeField] protected float vulnerabilityDuration = 5f;
    protected int numberOfPhases;
    protected int currentPhase;
    protected int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }
    protected bool isActive = false;
    protected bool isDefeated = false;
    protected bool isInmune = false;

    protected virtual void Start()
    {
        numberOfPhases = phasesData.Count;
        currentHealth = maxHealth;
        currentPhase = 0;
        animationManager.SetBossPhase(currentPhase);
        isActive = false;
        isDefeated = false;
        isInmune = true;

        ActivateBoss();
    }

    public void ActivateBoss()
    {
        //Falta la animación de entrar en combate
        isActive = true;
        animationManager.InitCombat();
    }

    public void SetVulnerable()
    {
        isInmune = false;
        animationManager.SetInmune(isInmune);
    }

    public void SetInmune()
    {
        isInmune = true;
        animationManager.SetInmune(isInmune);
    }

    public void TakeDamage(int damage)
    {
        //Double check, maybe not necessary
        if (isDefeated || !isActive || isInmune)
            return;

        currentHealth -= damage;

        Debug.Log("Boss took damage: " + damage + ", current health: " + currentHealth);

        CheckChangingPhase();

        if (currentHealth <= 0)
        {
            isDefeated = true;
            currentHealth = 0;
            animationManager.SetDead();

            Debug.Log("Boss Defeated");
        }
    }

    protected virtual void CheckChangingPhase()
    {
        for (int i = currentPhase; i++ < numberOfPhases; i++)
        {
            if(i + 1 >= numberOfPhases)
            {
                return; 
            }

            if (currentHealth <= phasesData[i + 1].healthThreshold)
            {
                ChangeToPhase(i + 1);
                return;
            }
        }
    }

    protected virtual void ChangeToPhase(int newPhase)
    {
        //Maybe more logic here later
        currentPhase = newPhase;
        animationManager.SetBossPhase(currentPhase);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            NewTestBullet bullet = collision.gameObject.GetComponent<NewTestBullet>();
            RecieveBulletHit(bullet);
        }
    }


    //These two methods control Boss behavior when receiving hits
    public abstract void RecieveBulletHit(NewTestBullet bullet);
    public abstract void RecieveMeleeHit(PlayerMelee meleeAttack);


    //! Y si quiero hacerlo muuuuy escalable/configurable?
    //! No serviria para boss con más de un miembro
        // public void TestDeEjecucionDeSubmetodos()
    // {
    //     List<Action> submetodos = new List<Action>()
    //     {
    //         () => RecieveBulletHit(null),
    //         () => RecieveMeleeHit(null)
    //     };

    //     foreach (var metodo in submetodos)
    //     {
    //         metodo.Invoke();
    //     }
    // }

}
