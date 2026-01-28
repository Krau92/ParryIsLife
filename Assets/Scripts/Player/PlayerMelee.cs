using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] GameObject meleeObject;
    
    [Header("Melee Settings")]
    [SerializeField] private int maxMeleeChargeLevel = 3;
    [SerializeField] float meleeDuration = 0.3f;
    [SerializeField] private float meleeCooldown = 1f;
    [SerializeField] private int parriedBulletToCharge = 10;

    private bool alreadyHit = false;
    private float meleeTimer = 0f;
    private int parriedBulletsCount = 0;
    private int currentMeleeChargeLevel = 0;
    public int GetCurrentMeleeChargeLevel() { return currentMeleeChargeLevel; }

    void OnEnable()
    {
        PlayerHealth.OnParriedBullet += CountParriedBullet;
        InputManager.onMeleeInput += StartMelee;
    }

    void OnDisable()
    {
        PlayerHealth.OnParriedBullet -= CountParriedBullet;
        InputManager.onMeleeInput -= StartMelee;
    }

    void Update()
    {
        if (meleeTimer > 0)
        {
            meleeTimer -= Time.deltaTime;
        }
    }

    void Start()
    {
        StopMelee();
    }

    public void StartMelee()
    {
        if (meleeTimer <= 0)
        {
            meleeObject.SetActive(true);
            Invoke("StopMelee", meleeDuration);
        }
    }

    private void StopMelee()
    {
        alreadyHit = false;
        meleeTimer = meleeCooldown;
        currentMeleeChargeLevel = 0;
        meleeObject.SetActive(false);
    }

    public void HandleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy") && !alreadyHit)
        {
            alreadyHit = true;
            other.GetComponent<Boss>().RecieveMeleeHit(this);
        }
    }


    public void CountParriedBullet()
    {
        parriedBulletsCount++;

        if (parriedBulletsCount >= parriedBulletToCharge)
        {
            parriedBulletsCount = 0;
            AddMeleeChargeLevel();
        }
        
    }

    private void AddMeleeChargeLevel()
    {
        if (currentMeleeChargeLevel < maxMeleeChargeLevel)
        {
            currentMeleeChargeLevel++;
        }
    }


}
