using UnityEngine;

public class PinpointInteraction : MonoBehaviour
{
    public string bossName;
    [SerializeField] bool isPlayerInRange = false;

    [SerializeField] BossBank bossBank;
    public UIBossInfo bossInfoPanel;
    public PinpointInteraction prevBossRequired;
    public int totalThreatRequired;


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
            bossInfoPanel.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerMap"))
        {
            isPlayerInRange = true;
            if(bossInfoPanel != null)
            {
                bossInfoPanel.gameObject.SetActive(true);
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
                bossInfoPanel.gameObject.SetActive(false);
            }
        }
    }

    private void OnConfirmInputDown()
    {
        if(isPlayerInRange && GameManager.Instance.currentGameState == GameState.InMap)
        {
            
            GameObject bossPrefab = bossBank.GetBossPrefab(bossName);
            GameEvents.OnBossSelected?.Invoke(bossPrefab);

        }
    }

    public bool IsBossCompleted()
    {
        return bossInfoPanel != null && bossInfoPanel.IsCompleted();
    }

    public void CheckRequirement()
    {
        bossInfoPanel.UpdateUI();
        if(prevBossRequired != null)
        {
            if(!prevBossRequired.IsBossCompleted())
            {
                gameObject.SetActive(false);
                return;
            }
        }
        if (bossInfoPanel.saveState.GetTotalThreatLevel() < totalThreatRequired)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
    }
}
