using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[Serializable]
public class BossPhaseData
{
    public string phaseName; //To organize and make easier to identify
    public float healthThreshold;
}

[Serializable]
public class BossPattern
{
    public ShootingPatternSO shootingPattern;
    public Transform shootOrigin;
    public float waitTime;
}

[Serializable]
public class BossComboPattern
{
    public string comboName; //To organize and make easier to identify
    public List<BossPattern> shootingPatterns;
    public int animationIndex;
    public float waitBetweenCombosTime;
}


public abstract class Boss : MonoBehaviour
{
    [SerializeField] protected BossAnimationManaging animationManager;
    protected Transform playerTransform;
    [SerializeField] protected int maxHealth;
    public int MaxHealth { get { return maxHealth; } }
    [SerializeField] protected List<BossPhaseData> phasesData;
    [SerializeField] protected List<BossComboPattern> comboPatterns;
    [SerializeField] protected float vulnerabilityDuration = 5f;

    [SerializeField] protected float timeToStartAttacking = 2f;
    
    protected int numberOfPhases;
    protected int currentPhase;
    protected int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }
    protected bool isActive = false;
    protected bool isDefeated = false;
    protected bool isInmune = false;
    protected bool isAttacking = false;

    protected List<BossComboPattern> currentCombos;
    protected int currentPatternIndex = 0;
    protected int currentComboIndex = 0;

    protected Coroutine bossBehaviorCoroutine;
    protected Coroutine shootingCoroutine;
    protected bool comboFinished;

    protected virtual void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        numberOfPhases = phasesData.Count;
        currentHealth = maxHealth;
        currentPhase = 0;
        currentPatternIndex = 0;
        currentComboIndex = 0;
        isActive = false;
        isDefeated = false;
        isInmune = true;
        isAttacking = false;

        ActivateBoss();
    }

    public virtual void ActivateBoss()
    {
        isActive = true;
        animationManager.InitCombat();
        StartBossBehavior();
    }

    protected void StartBossBehavior()
    {
        StopBossBehavior();
        if (!isDefeated && isActive)
        {
            bossBehaviorCoroutine = StartCoroutine(BossBehaviorLoop());
        }
    }

    protected void StopBossBehavior()
    {
        if (bossBehaviorCoroutine != null)
        {
            StopCoroutine(bossBehaviorCoroutine);
            bossBehaviorCoroutine = null;
        }

        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    protected IEnumerator BossBehaviorLoop()
    {
        yield return new WaitForSeconds(timeToStartAttacking);

        while (isActive && !isDefeated)
        {
            // === STATE: PreparingShoot ===
            PrepareShootState();
            yield return null; 

            // === STATE: ExecutingAnimation ===
            // Iniciamos la animación que eventualmente llamará a ShootCombo() por evento
            comboFinished = false; // Reseteamos flag
            SetAnimation();
            
            // === STATE: ShootingCombo ===
            // Esperamos a que el combo termine (activado y gestionado por ShootComboRoutine llamado desde evento)
            
            // Esperamos hasta que ShootComboRoutine marque comboFinished = true
            // OPCIONAL: TimeOut de seguridad por si el evento de animación nunca salta podría añadirse aquí
            yield return new WaitUntil(() => comboFinished);

            // === STATE: ChoosingNextCombo ===
            PrepareNextComboState();
            
            BossComboPattern currentCombo = currentCombos[currentComboIndex];
            yield return new WaitForSeconds(currentCombo.waitBetweenCombosTime);
        }
    }

    public void SetVulnerable()
    {
        isInmune = false;
        animationManager.SetInmune(isInmune);
        animationManager.StopAttacking();
        StopBossBehavior();
    }

    public void SetInmune()
    {
        isInmune = true;
        animationManager.SetInmune(isInmune);
        StartBossBehavior();
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
            StopBossBehavior();
            animationManager.StopAttacking();
            isDefeated = true;
            currentHealth = 0;
            animationManager.SetDead();

            Debug.Log("Boss Defeated");
        }
    }

    protected virtual void CheckChangingPhase()
    {
        if (currentPhase >= phasesData.Count - 1) return;

        BossPhaseData nextPhase = phasesData[currentPhase + 1];

        if (currentHealth <= nextPhase.healthThreshold)
        {
            ChangeToPhase(currentPhase + 1);
        }
    }

    //!This is the method to override to prepare the current patterns/combos for each phase for each specific boss
    protected abstract void ChangeToPhase(int newPhase);

    protected virtual void PrepareShootState()
    {
        if (currentCombos != null && currentCombos.Count > currentComboIndex)
        {
            animationManager.SetBossAnimation(currentCombos[currentComboIndex].animationIndex);
            animationManager.StopAttacking();
        }
    }

    protected virtual void SetAnimation()
    {
        animationManager.StartAttacking();
    }

    // Método llamado por EVENTO DE ANIMACIÓN
    public virtual void ShootCombo()
    {
        if(isAttacking) return; // Prevent overlapping attacks
        isAttacking = true;
        if (shootingCoroutine != null) StopCoroutine(shootingCoroutine);
        shootingCoroutine = StartCoroutine(ShootComboRoutine());
    }

    protected virtual IEnumerator ShootComboRoutine()
    {
        BossComboPattern currentCombo = currentCombos[currentComboIndex];
        currentPatternIndex = 0;

        while (currentPatternIndex < currentCombo.shootingPatterns.Count)
        {
            BossPattern currentData = currentCombo.shootingPatterns[currentPatternIndex];
            ShootingPatternSO currentPattern = currentData.shootingPattern;

            StartCoroutine(currentPattern.Shoot(
                currentData.shootOrigin.position, 
                playerTransform.position, 
                currentPattern.prefabBullet)
            );

            yield return new WaitForSeconds(currentData.waitTime);
            isAttacking = false;
            currentPatternIndex++;
        }

        isAttacking = false;
        comboFinished = true; // Notificamos al bucle principal que hemos terminado
        shootingCoroutine = null;
    }

    //!THIS IS THE METHOD TO OVERRIDE TO DECIDE NEXT COMBO FOR EACH ESPECIFIC BOSS
    //Standard behaviour: shoot combos in order
    protected virtual void PrepareNextComboState()
    {
        if(currentComboIndex == currentCombos.Count - 1)
            currentComboIndex = 0;
        else
            currentComboIndex++;
            
        currentPatternIndex = 0;
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

}
