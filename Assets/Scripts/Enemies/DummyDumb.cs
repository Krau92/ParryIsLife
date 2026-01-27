using System;
using System.Collections.Generic;
using UnityEngine;

public class DummyDumb : Boss
{
    public override void RecieveBulletHit(NewTestBullet bullet)
    {
        if (!isDefeated && isActive)
        {   //If inmune check if set vulnerable
            if (isInmune)
            {
                if (bullet.IsReflected())
                {
                    SetVulnerable();
                    Invoke("SetInmune", vulnerabilityDuration);
                }
                bullet.DeactivateBullet();
                return;
            }

            //Handling damages
            TakeDamage(1);

            if (bullet.IsCharged())
                TakeDamage(4);

            if (bullet.IsReflected())
                TakeDamage(1);
        }

        bullet.DeactivateBullet();
    }



    public override void RecieveMeleeHit(PlayerMelee meleeAttack)
    {
        if (isInmune || isDefeated || !isActive)
            return;

        TakeDamage(3 + meleeAttack.GetCurrentMeleeChargeLevel() * 3);
    }




}
