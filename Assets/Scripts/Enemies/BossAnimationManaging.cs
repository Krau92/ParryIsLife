using UnityEngine;

public class BossAnimationManaging : MonoBehaviour
{
    [SerializeField] private Animator bossAnimator;

    public void InitCombat()
    {
        bossAnimator.SetBool("Active", true);
        bossAnimator.SetBool("Dead", false);
        bossAnimator.SetBool("Inmune", true);
    }

    public void SetBossPhase(int phase)
    {
        bossAnimator.SetInteger("BossPhase", phase);
    }

    public void SetDead()
    {
        bossAnimator.SetBool("Dead", true);

    }

    public void SetInmune(bool inmune)
    {
        bossAnimator.SetBool("Inmune", inmune);
    }
}
