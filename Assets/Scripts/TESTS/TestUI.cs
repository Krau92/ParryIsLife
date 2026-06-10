using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private PlayerMelee playerMelee;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerParry playerParry;


    [SerializeField] Image bossHealthBarFill;
    [SerializeField] Image playerHealthBarFill;
    [SerializeField] Image parryChargeBarFill;
    [SerializeField] TMP_Text lvlMelee;

    void OnEnable()
    {
        CombatEvents.OnPlayerCreated += GetPlayer;
    }

    void OnDisable()
    {
        CombatEvents.OnPlayerCreated -= GetPlayer;
    }

    void GetPlayer(GameObject playerObj)
    {
        playerMelee = playerObj.GetComponent<PlayerMelee>();
        playerHealth = playerObj.GetComponent<PlayerHealth>();
        playerParry = playerObj.GetComponent<PlayerParry>();
    }


    private void Update()
    {
        if (boss != null && bossHealthBarFill != null)
        {
            float healthPercentage = (float)boss.CurrentHealth / boss.MaxHealth;
            bossHealthBarFill.fillAmount = healthPercentage;
        }

        if (playerMelee != null && lvlMelee != null)
        {
            lvlMelee.text = "Melee LvL: " + playerMelee.GetCurrentMeleeChargeLevel().ToString();
        }

        if (playerHealth != null && playerHealthBarFill != null)
        {
            float healthPercentage = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
            playerHealthBarFill.fillAmount = healthPercentage;
        }

        if (playerParry != null && parryChargeBarFill != null)
        {
            float parryChargeAmount = playerParry.GetChargePercentage();
            parryChargeBarFill.fillAmount = parryChargeAmount;
        }
    }
}
