using UnityEngine;

public class BossAnimationManaging : MonoBehaviour
{
    [SerializeField] private Animator bossAnimator;

    public void InitCombat()
    {
        bossAnimator.SetBool("Active", true);
        bossAnimator.SetBool("Dead", false);
        bossAnimator.SetBool("Inmune", true);
        bossAnimator.SetBool("Attacking", false);
    }

    public void StartAttacking()
    {
        Debug.Log("Boss starts attacking animation.");
        bossAnimator.SetBool("Attacking", true);
    }

    public void SetBossAnimation(int anim)
    {
        bossAnimator.SetInteger("Animation", anim);
    }

    public void SetDead()
    {
        bossAnimator.SetBool("Dead", true);
        bossAnimator.SetBool("Attacking", false);
    }

    public void SetInmune(bool inmune)
    {
        bossAnimator.SetBool("Inmune", inmune);
    }

    public void StopAttacking()
    {
        Debug.Log("Boss stops attacking animation.");
        bossAnimator.SetBool("Attacking", false);
    }
}
