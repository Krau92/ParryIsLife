using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class DummyDumb : Boss
{

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


}
