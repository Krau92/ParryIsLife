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
    public List<int> availableCombosIndexes;
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
    [Header("Setting up")]
    [SerializeField] protected BossAnimationManaging animationManager;
    protected Transform playerTransform;
    
    [SerializeField] protected float timeToStartAttacking = 2f;
    [SerializeField] int[] threatLevelThresholds = new int[3];
    public int[] ThreatLevelThresholds { get { return threatLevelThresholds; } }

    [Header("Boss Stats")]
    [SerializeField] protected int maxHealth;
    public int MaxHealth { get { return maxHealth; } }
    [SerializeField] protected int initBulletStunCounter = 5;
    [SerializeField] protected float vulnerabilityDuration = 5f;

    [Header("Phases and Combos")]
    [SerializeField] protected List<BossPhaseData> phasesData;
    [SerializeField] protected List<BossComboPattern> comboPatterns;


    [Header("Damage Settings")]
    [SerializeField] protected float stunDmgMultiplier = 2.5f;
    [SerializeField] protected int bulletBaseDmg = 1;
    [SerializeField] protected int chargedBulletDmg = 2;
    [SerializeField] protected int reflectedBulletDmg = 1;
    [SerializeField] protected int[] meleeLvlDmg = new int[4];


    protected int currentBulletStunCounter;
    protected float dmgMultiplier = 1f;
    
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
        ChangeToPhase(0); //Start in phase 0
        currentPatternIndex = 0;
        currentComboIndex = 0;
        currentBulletStunCounter = initBulletStunCounter;
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

    protected virtual void StopBossBehavior()
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
            PrepareShootState();
            yield return null; 

            comboFinished = false; // Reseteamos flag para entrar limpio a la animación y que pueda lanzar el combo
            animationManager.StartAttacking();
            
            //Aqui esperamos a que la animación dispare todo el combo
            yield return new WaitUntil(() => comboFinished);

            BossComboPattern currentCombo = currentCombos[currentComboIndex];
            yield return new WaitForSeconds(currentCombo.waitBetweenCombosTime);

            PrepareNextComboState();
                        
        }
    }

    public void SetVulnerable()
    {
        CombatEvents.OnEnemyStunned?.Invoke();
        isInmune = false;
        animationManager.SetInmune(isInmune);
        dmgMultiplier = stunDmgMultiplier;
        StopAttacking();
        StopBossBehavior();
    }

    public void SetInmune()
    {
        isInmune = true;
        animationManager.SetInmune(isInmune);
        currentBulletStunCounter = initBulletStunCounter;
        dmgMultiplier = 1f;
        StartBossBehavior();
    }

    public void TakeDamage(float damage)
    {
        //Double check, maybe not necessary
        if (isDefeated || !isActive)
            return;
        int intDamage = Mathf.RoundToInt(damage);
        currentHealth -= intDamage;
        CombatEvents.OnBossDamaged?.Invoke();

        CheckChangingPhase();

        if (currentHealth <= 0)
        {
            StopAttacking();
            StopBossBehavior();
            isDefeated = true;
            currentHealth = 0;
            animationManager.SetDead();

            CombatEvents.OnBossDefeated?.Invoke();

        }
    }

    protected virtual void CheckChangingPhase()
    {
        if (currentPhase >= phasesData.Count - 1) return;

        BossPhaseData nextPhase = phasesData[currentPhase + 1];
        int healthThreshold = Mathf.RoundToInt(maxHealth * nextPhase.healthThreshold / 100f);

        if (currentHealth <= healthThreshold)
        {
            ChangeToPhase(currentPhase + 1);
        }
    }

    protected void ChangeToPhase(int newPhase)
    {
        StopAttacking();
        StopBossBehavior();
        currentPhase = newPhase;
        BossPhaseData phaseData = phasesData[currentPhase];
        List<BossComboPattern> newCombos = new List<BossComboPattern>();

        foreach (int comboIndex in phaseData.availableCombosIndexes)
        {
            if (comboIndex >= 0 && comboIndex < comboPatterns.Count)
            {
                newCombos.Add(comboPatterns[comboIndex]);
            }
        }

        currentCombos = newCombos;
        currentComboIndex = 0;
        currentPatternIndex = 0;

        StartBossBehavior();
    }

    protected virtual void PrepareShootState()
    {
        if (currentCombos != null && currentCombos.Count > currentComboIndex)
        {
            currentPatternIndex = 0;
            StopAttacking();
            animationManager.SetBossAnimation(currentCombos[currentComboIndex].animationIndex);
        }
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

        while (currentPatternIndex < currentCombo.shootingPatterns.Count && isInmune)
        {
            BossPattern currentData = currentCombo.shootingPatterns[currentPatternIndex];
            ShootingPatternSO currentPattern = currentData.shootingPattern;

            StartCoroutine(currentPattern.Shoot(
                currentData.shootOrigin.position, 
                playerTransform.position, 
                currentPattern.prefabBullet)
            );

            yield return new WaitForSeconds(currentData.waitTime);
            currentPatternIndex++;
        }
        StopAttacking();
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

    protected void StopAttacking()
    {
        isAttacking = false;
        animationManager.StopAttacking();
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }


    //These two methods control Boss behavior when receiving hits
    public abstract void RecieveBulletHit(NewTestBullet bullet);
    public abstract void RecieveMeleeHit(PlayerMelee meleeAttack);

}
