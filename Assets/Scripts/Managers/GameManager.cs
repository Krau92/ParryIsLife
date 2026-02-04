using UnityEngine;


public enum GameState
{
    MainMenu,
    InMap,
    InCombat,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentGameState;
    [SerializeField] private Camera mapCamera;
    [SerializeField] private Camera combatCamera;
    [SerializeField] private GameState initialGameState;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetGameState(initialGameState);
    }

    public void SetGameState(GameState newState)
    {
        currentGameState = newState;
        //Handle state transition logic here (e.g., pausing the game, showing UI, etc.)
    }
}
