using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private PlayerMelee playerMelee;
    [SerializeField] private PlayerHealth playerHealth;


    [SerializeField] Image bossHealthBarFill;
    [SerializeField] Image playerHealthBarFill;
    [SerializeField] TMP_Text lvlMelee;


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
    }
}
