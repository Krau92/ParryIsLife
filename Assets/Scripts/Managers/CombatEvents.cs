using UnityEngine;
using System;

public static class CombatEvents
{
    public static Action OnChargingParryStart;
    public static Action OnChargingParryEnd;
    public static Action OnParryStart;
    public static Action OnParryEnd;
    public static Action OnReflectingStart;
    public static Action OnReflectingEnd;
    public static Action OnChargingShotStart;
    public static Action OnChargingShotEnd;
    public static Action<bool> OnPlayerShoot;

    
    public static Action<Boss> OnBossSelected;

    public static Action OnDamageTaken;
    public static Action OnPlayerDeath;
    public static Action OnBossDamaged;
    public static Action OnBossDefeated;
    
    public static Action OnParriedBullet;
    public static Action OnReflectedBullet;
    public static Action<int> OnMeleeHit;
    public static Action OnEnemyStunned;

}
