using UnityEngine;

public class PinpointInteraction : MonoBehaviour
{
    public string bossName;
    [SerializeField] bool isPlayerInRange = false;

    [SerializeField] BossBank bossBank;
    public GameObject bossInfoPanel;

    void OnEnable()
    {
        InputManager.onShootInput += OnConfirmInputDown;
    }

    void OnDisable()
    {
        InputManager.onShootInput -= OnConfirmInputDown;
    }

    void Start()
    {
        if (bossInfoPanel != null)
        {
            bossInfoPanel.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMap"))
        {
            isPlayerInRange = true;
            if(bossInfoPanel != null)
            {
                bossInfoPanel.SetActive(true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMap"))
        {
            isPlayerInRange = false;
            if(bossInfoPanel != null)
            {
                bossInfoPanel.SetActive(false);
            }
        }
    }

    private void OnConfirmInputDown()
    {
        if(isPlayerInRange && GameManager.Instance.currentGameState == GameState.InMap)
        {
            
            GameObject bossPrefab = bossBank.GetBossPrefab(bossName);
            CombatEvents.OnBossSelected?.Invoke(bossPrefab);

        }
    }
}
