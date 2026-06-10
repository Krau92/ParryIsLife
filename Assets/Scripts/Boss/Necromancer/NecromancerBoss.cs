using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerBoss : Boss
{
    [SerializeField] private SoundEffectSO hitSoundEffect;
    [SerializeField] private float soundEffectCooldown = 0.5f;
    [SerializeField] private GameObject minionPrefab;
    bool damageable = false;
    public List<MinionBarrier> minionBarrier = new List<MinionBarrier>();
    public List<Transform> minionPositionPoints = new List<Transform>();

    private float nextHitSoundEffectTime = 0f;
    public override void RecieveBulletHit(NewTestBullet bullet)
    {
        if (!isDefeated && isActive)
        {   //If inmune check if set vulnerable
            if (!damageable)
            {

                return;
            }
            float currentDmgMultiplier = dmgMultiplier;
            if (isInmune)
            {
                currentDmgMultiplier = 1f;
            }

            //Handling damages
            TakeDamage(bulletBaseDmg * currentDmgMultiplier);

            if (bullet.IsCharged())
                TakeDamage(chargedBulletDmg * currentDmgMultiplier);

            if (bullet.IsReflected())
                TakeDamage(reflectedBulletDmg * currentDmgMultiplier);
        }

        if (nextHitSoundEffectTime <= Time.time)
        {
            if (hitSoundEffect != null)
            {
                hitSoundEffect.PlayEffect();
            }
            nextHitSoundEffectTime = Time.time + soundEffectCooldown;
        }

    }

    protected override void ChangeToPhase(int newPhase)
    {
        damageable = false;
        SpawnMinionBarriers(newPhase + 2);
        if (newPhase == 2)
        {
            SetMayhem();
        }
        StopMayhem();
        base.ChangeToPhase(newPhase);
    }


    public override void RecieveMeleeHit(PlayerMelee meleeAttack)
    {
        if (isDefeated || !isActive || !damageable)
            return;

        TakeDamage(meleeLvlDmg[meleeAttack.GetCurrentMeleeChargeLevel()] * dmgMultiplier);
    }

    protected override void StopBossBehavior()
    {
        base.StopBossBehavior();
        currentComboIndex = 0;
        currentPatternIndex = 0;
    }


    void SetMayhem()
    {
        foreach (BossComboPattern pattern in comboPatterns)
        {
            pattern.waitBetweenCombosTime *= 0.5f;
        }
    }

    void StopMayhem()
    {
        foreach (BossComboPattern pattern in comboPatterns)
        {
            pattern.waitBetweenCombosTime *= 2f;
        }
    }

    #region minion management
    public void OnMinionBarrierDestroyed(MinionBarrier minion)
    {
        minionBarrier.Remove(minion);
        Destroy(minion.gameObject);
        if (minionBarrier.Count <= 0 && isInmune)
        {
            damageable = true;
            SetVulnerable();
            Invoke("SetInmune", vulnerabilityDuration);
            SetMayhem();
        }
        else if (minionBarrier.Count > 0)
        {
            for (int i = 0; i < minionBarrier.Count; i++)
            {
                minionBarrier[i].SetNewPosition(minionPositionPoints[i]);
            }
        }
    }

    void SpawnMinionBarriers(int qty)
    {
        for (int i = 0; i < qty; i++)
        {
            MinionBarrier newMinion = Instantiate(minionPrefab, transform.position, Quaternion.identity).GetComponent<MinionBarrier>();
            newMinion.necromancerBoss = this;
            newMinion.playerTransform = playerTransform;
            newMinion.SetHealth((qty + 4) * 2f);
            newMinion.SetNewPosition(minionPositionPoints[i]);
            minionBarrier.Add(newMinion);
        }
    }

    #endregion


}
