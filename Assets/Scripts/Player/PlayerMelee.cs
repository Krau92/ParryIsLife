using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] GameObject meleeObject;
    
    [Header("Melee Settings")]
    [SerializeField] private int maxMeleeChargeLevel = 3;
    [SerializeField] float meleeDuration = 0.75f;
    // [SerializeField] private float meleeCooldown = 1f;
    [SerializeField] private int parriedBulletToCharge = 10;
    bool attacking = false;
    private int parriedBulletsCount = 0;
    private int currentMeleeChargeLevel = 0;
    private MeleeCollisions col;

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

    void Start()
    {
        col = meleeObject.GetComponent<MeleeCollisions>();
        StopMelee();
    }

    public void StartMelee()
    {
        //TODO: Pasar el nivel de carga al daño del melee

        attacking = true;
        meleeObject.SetActive(true);
        Invoke("StopMelee", meleeDuration);
        currentMeleeChargeLevel = 0;        
    }

    private void StopMelee()
    {
        attacking = false;
        meleeObject.SetActive(false);
    }

    public void HandleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Handle enemy damage based on currentMeleeChargeLevel
            Debug.Log("Hit enemy with melee at charge level: " + currentMeleeChargeLevel);
        }
    }


    public void CountParriedBullet()
    {
        parriedBulletsCount++;
        Debug.Log("Parried bullets count: " + parriedBulletsCount);

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
            Debug.Log("Melee charge level increased to: " + currentMeleeChargeLevel);
        }
    }


}
