using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MikeWazowsky : Boss
{
    [SerializeField] private SoundEffectSO hitSoundEffect;
    [SerializeField] private float soundEffectCooldown = 0.5f;
    [SerializeField] private GameObject tonguePrefab;
    public List<Transform> tongueSpawnPoints = new List<Transform>();
    public List<Tongue> tongues = new List<Tongue>();

    private float nextHitSoundEffectTime = 0f;

    public override void ActivateBoss()
    {
        currentCombos = comboPatterns;
        base.ActivateBoss();
        currentBulletStunCounter = initBulletStunCounter;
    }
    public override void RecieveBulletHit(NewTestBullet bullet)
    {
        if (!isDefeated && isActive)
        {   //If inmune check if set vulnerable
            if (isInmune)
            {
                if (bullet.IsReflected())
                {
                    currentBulletStunCounter--;
                    if (currentBulletStunCounter <= 0)
                    {
                        SetVulnerable();
                        Invoke("SetInmune", vulnerabilityDuration);
                    }
                }
            } 

            //Handling damages
            TakeDamage(bulletBaseDmg * dmgMultiplier);

            if (bullet.IsCharged())
                TakeDamage(chargedBulletDmg * dmgMultiplier);

            if (bullet.IsReflected())
                TakeDamage(reflectedBulletDmg * dmgMultiplier);
        }
        
        if(nextHitSoundEffectTime <= Time.time)
        {
            if(hitSoundEffect != null)
            {
                hitSoundEffect.PlayEffect();
            }
            nextHitSoundEffectTime = Time.time + soundEffectCooldown;
        }

        bullet.DeactivateBullet();
    }

    public override void RecieveMeleeHit(PlayerMelee meleeAttack)
    {
        if (isDefeated || !isActive)
            return;

        TakeDamage(meleeLvlDmg[meleeAttack.GetCurrentMeleeChargeLevel()] * dmgMultiplier);
    }

    protected override void StopBossBehavior()
    {
        base.StopBossBehavior();
        currentComboIndex = 0;
        currentPatternIndex = 0;
    }

    public void ActivateTongues()
    {
        isAttacking = true;
        foreach(Tongue tongue in tongues)
        {
            tongue.StartSeeking();
        }
    }

    public void StopTongues()
    {
        foreach(Tongue tongue in tongues)
        {
            tongue.StopSeeking();
        }
    }

    public void ShootWithTongue()
    {
        foreach(Tongue tongue in tongues)
        {
            BossPattern pattern = currentCombos[currentComboIndex].shootingPatterns[currentPatternIndex];
            tongue.Attack(pattern.shootingPattern, pattern.shootingPattern.prefabBullet);
        }
        StopAttacking();
        comboFinished = true;
    }


}
