using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private SaveStateSO saveStateSO;

    //!TEST
    [SerializeField] private Boss testBoss;

    // [Header("Combat Feedback Parameters")]
    // [SerializeField] private float hitPauseDuration = 0.05f;
    // [SerializeField] private float hitPauseDurationMultiplier = 2f;
    // [SerializeField] private float slowDownTimeScale = 0.1f;
    // [SerializeField] private float slowDownDuration = 0.1f;

    // float originalDeltaTime;

    [Header("Scoring Parameters")]
    [SerializeField] private int deductedPointsOnDamage = 10;
    [SerializeField] private int pointsPerParry = 5;
    [SerializeField] private int pointsPerReflect = 1;
    [SerializeField] private int[] pointsPerMeleeHit = new int[4] { 0, 5, 15, 30 };
    [SerializeField] private int pointsOnEnemyStun = 10;


    CombatResult combatResult = new CombatResult();
    public CombatResult CombatResults => combatResult;
    int[] threatLevelThresholds = new int[3];


    void OnEnable()
    {
        //!TEST
        BossSelected(testBoss); // Para pruebas, asigna un boss al iniciar el juego. Elimina esta línea en producción.


        // originalDeltaTime = Time.fixedDeltaTime;
        CombatEvents.OnDamageTaken += OnDamageTaken;
        CombatEvents.OnParriedBullet += OnParriedBullet;
        CombatEvents.OnReflectedBullet += OnReflectedBullet;
        CombatEvents.OnMeleeHit += OnMeleeHit;
        CombatEvents.OnEnemyStunned += OnEnemyStunned;
        CombatEvents.OnBossSelected += BossSelected;
        CombatEvents.OnBossDefeated += OnBossDefeated;
        CombatEvents.OnPlayerDeath += EndCombat;
    }

    void OnDisable()
    {
        CombatEvents.OnDamageTaken -= OnDamageTaken;
        CombatEvents.OnParriedBullet -= OnParriedBullet;
        CombatEvents.OnReflectedBullet -= OnReflectedBullet;
        CombatEvents.OnMeleeHit -= OnMeleeHit;
        CombatEvents.OnEnemyStunned -= OnEnemyStunned;
        CombatEvents.OnBossSelected -= BossSelected;
        CombatEvents.OnBossDefeated -= OnBossDefeated;
        CombatEvents.OnPlayerDeath -= EndCombat;
    }

    // private Coroutine activeHitStop;

    // private void HitStop(float duration, float timeScale)
    // {
    //     // Detener corrutina anterior para evitar conflictos si ocurren eventos simultáneos
    //     if (activeHitStop != null)
    //         StopCoroutine(activeHitStop);

    //     Time.timeScale = timeScale;

    //     // NUNCA asignar 0 a fixedDeltaTime, causa inestabilidad en físicas (saltos)
    //     if (timeScale > 0f)
    //     {
    //         Time.fixedDeltaTime = originalDeltaTime * Time.timeScale;
    //     }

    //     activeHitStop = StartCoroutine(WaitAndResetTimeScale(duration));
    // }

    // private IEnumerator WaitAndResetTimeScale(float duration)
    // {
    //     yield return new WaitForSecondsRealtime(duration);
    //     ResetTimeScale();
    // }

    // private void ResetTimeScale()
    // {
    //     Time.timeScale = 1f;
    //     Time.fixedDeltaTime = originalDeltaTime;
    // }



    private void BossSelected(Boss boss)
    {
        threatLevelThresholds = boss.ThreatLevelThresholds;
        combatResult = new CombatResult
        {
            bossName = boss.name,
            completed = false,
            maxScore = 0,
            threatLevel = 0
        };
    }

    private void OnDamageTaken()
    {
        combatResult.maxScore = Mathf.Max(0, combatResult.maxScore - deductedPointsOnDamage);

    }

    private void OnParriedBullet()
    {
        combatResult.maxScore += pointsPerParry;
    }
    private void OnReflectedBullet()
    {
        combatResult.maxScore += pointsPerReflect;
        //! No se que feedback poner aqui
    }

    private void OnMeleeHit(int chargedMeleeLevel)
    {
        combatResult.maxScore += pointsPerMeleeHit[chargedMeleeLevel];

    }

    private void OnEnemyStunned()
    {
        combatResult.maxScore += pointsOnEnemyStun;
    }




    private void OnBossDefeated()
    {
        combatResult.completed = true;
        EndCombat();
        GameManager.Instance.SetGameState(GameState.ShowingResults);
    }

    private void EndCombat()
    {
        if (combatResult.completed)
        {
            // Determine threat level based on maxScore and thresholds
            if (combatResult.maxScore >= threatLevelThresholds[2])
            {
                combatResult.threatLevel = 3;
            }
            else if (combatResult.maxScore >= threatLevelThresholds[1])
            {
                combatResult.threatLevel = 2;
            }
            else if (combatResult.maxScore >= threatLevelThresholds[0])
            {
                combatResult.threatLevel = 1;
            }
            else
            {
                combatResult.threatLevel = 0;
            }
        }

        saveStateSO.AddOrUpdateCombatResult(combatResult);
        GameManager.Instance.SetGameState(GameState.ShowingResults);

    }

}
