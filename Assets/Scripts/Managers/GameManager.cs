using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using System;


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
    public Transform bossSpawnPoint, playerSpawnPoint;
    public GameObject playerPrefab, playerInstance, bossInstance;
    [SerializeField] private AspectRatioEnforcer aspectRatioEnforcer;
    [SerializeField] private CinemachineCamera combatCamera, mapCamera;



    // Propiedad pública para leer, privada para escribir. Fuerza a usar SetGameState.
    public GameState currentGameState { get; private set; }

    // Evento estático para notificar cambios de estado sin necesidad de referencias directas
    public static event Action<GameState> OnGameStateChanged;

    [SerializeField] private GameState initialGameState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Hace que el Manager persista entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        CombatEvents.OnBossSelected += SpawnBoss;
        CombatEvents.OnResultsClosed += BackToMap;
    }

    void OnDisable()
    {
        CombatEvents.OnBossSelected -= SpawnBoss;
        CombatEvents.OnResultsClosed -= BackToMap;
    }

    void Start()
    {
        SetGameState(initialGameState);
    }

    public void SetGameState(GameState newState)
    {

        if (currentGameState == newState) return;

        if (currentGameState == GameState.InCombat && newState == GameState.ShowingResults)
        {
            CombatEvents.OnCombatEnded?.Invoke();
            if (playerInstance != null)
            {
                Destroy(playerInstance);
            }

            if (bossInstance != null)
            {
                Destroy(bossInstance);
            }
        }

        currentGameState = newState;

        if (aspectRatioEnforcer != null)
        {
            if (currentGameState == GameState.InCombat)
            {
                aspectRatioEnforcer.SetCombatRatio();
                combatCamera.Priority = 10;
                mapCamera.Priority = 0;
            }
            else
            {
                aspectRatioEnforcer.SetNormalRatio();
                combatCamera.Priority = 0;
                mapCamera.Priority = 10;
            }
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void SpawnBoss(GameObject bossPrefab)
    {
        if (bossPrefab != null)
        {
            SetGameState(GameState.InCombat);
            StartCoroutine(SpawnBossWithDelay(bossPrefab, 0.1f)); 
        }
        else
        {
            Debug.LogError("Boss prefab is null. Cannot spawn boss.");
        }
    }

    IEnumerator SpawnBossWithDelay(GameObject bossPrefab, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerInstance != null)
        {
            Destroy(playerInstance);
        }
        playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);

        if (bossInstance != null)
        {
            Destroy(bossInstance);
        }
        bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity, bossSpawnPoint);
    }

    private void BackToMap()
    {
        SetGameState(GameState.InMap);
    }
}
