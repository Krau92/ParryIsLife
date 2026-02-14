using System; // Necesario para los eventos
using UnityEngine;


public enum GameState
{
    MainMenu,
    InMap,
    InCombat,
    ShowingResults,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Propiedad pública para leer, privada para escribir. Fuerza a usar SetGameState.
    public GameState currentGameState { get; private set; }

    // Evento estático para notificar cambios de estado sin necesidad de referencias directas
    public static event Action<GameState> OnGameStateChanged;

    [SerializeField] private GameState initialGameState;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Hace que el Manager persista entre escenas
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
        if (currentGameState == newState) return;

        currentGameState = newState;
        Debug.Log($"Game State changed to: {newState}");

        OnGameStateChanged?.Invoke(newState);
    }
}
