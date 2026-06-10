using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeWazowsky : Boss
{
    [SerializeField] private SoundEffectSO hitSoundEffect;
    [SerializeField] private float soundEffectCooldown = 0.5f;
    [SerializeField] private float pauseBetweenTongueAttacks = 0.5f;
    [SerializeField] private GameObject tonguePrefab;
    public GameObject seekingMarkPrefab;
    GameObject seekingMark;

    public Transform maxTongueSpawn, minTongueSpawn;
    public List<Tongue> tongues = new List<Tongue>();

    private float nextHitSoundEffectTime = 0f;
    protected override void Start()
    {
        seekingMark = Instantiate(seekingMarkPrefab, transform.position, seekingMarkPrefab.transform.rotation);
        seekingMark.SetActive(false);
        base.Start();
    }
    public override void ActivateBoss()
    {
        
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

    }

    protected override void ChangeToPhase(int newPhase)
    {
        switch (newPhase)
        {
            case 0:
                CreateTongues(1);
                break;
            case 1:
                CreateTongues(2);
                break;
            case 2:
                CreateTongues(4);
                break;

            default:
                break;
        }
        base.ChangeToPhase(newPhase);
    }

    private void CreateTongues(int numberOfTongues)
    {
        foreach(Tongue tongue in tongues)
        {
            Destroy(tongue.gameObject);
        }
        tongues.Clear();


        int a = 0;
        int b = 0;
        if(numberOfTongues % 2 != 0)
        {
            a = 1;
            b = 1;
        } else
        {
            a = 0;
            b = -1;
        }
        for(int i = 0; i < numberOfTongues; i++)
        {
            Vector3 spawnPosition = Vector3.Lerp(minTongueSpawn.position, maxTongueSpawn.position, (float)(i + a) / (numberOfTongues + b));
            GameObject tongueObj = Instantiate(tonguePrefab, spawnPosition, tonguePrefab.transform.rotation, transform);
            Tongue tongue = tongueObj.GetComponent<Tongue>();
            tongues.Add(tongue);
            tongue.seekingMark = seekingMark;
        }
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

    public void ShootWithTongues()
    {
        StartCoroutine(ShootWithTonguesCoroutine());
    }

    IEnumerator ShootWithTonguesCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(pauseBetweenTongueAttacks);
        foreach(Tongue tongue in tongues)
        {
            BossPattern pattern = currentCombos[currentComboIndex].shootingPatterns[currentPatternIndex];
            tongue.Attack(pattern.shootingPattern, pattern.shootingPattern.prefabBullet);
            yield return wait;
        }
        StopAttacking();
        comboFinished = true;
    }


}
