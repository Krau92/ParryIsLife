using UnityEngine;

public class PinpointInteraction : MonoBehaviour
{
    public string bossName;
    [SerializeField] bool isPlayerInRange = false;

    //!ELIMINAR,  LO HARÁ EL GAME MANAGER!
    [SerializeField] Camera bossCombatCamera;
    [SerializeField] BossBank bossBank;

    void OnEnable()
    {
        InputManager.onShootInput += OnConfirmInputDown;
    }

    void OnDisable()
    {
        InputManager.onShootInput -= OnConfirmInputDown;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered with tag: " + other.tag);
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger Exited with tag: " + other.tag);
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void OnConfirmInputDown()
    {
        if(isPlayerInRange && GameManager.Instance.currentGameState == GameState.InMap)
        {
            //GameManager.Instance.StartBossFight(bossName);
            //En el game manager gestionar la transición con cámara, posición de spawn del boss, recolocar jugador
            //Activar de nuevo el jugador y desactivar el avatar del mapamundi...
            //En un futuro añadir una UI previa para mostrar info del boss o bien doble confirmación
            
            GameObject bossPrefab = bossBank.GetBossPrefab(bossName);
            CombatEvents.OnBossSelected?.Invoke(bossPrefab.GetComponent<Boss>());

            //!Provisional
            Vector3 spawnPosition = new Vector3(0, 2.59f, 0);
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            bossCombatCamera.enabled = true;

            GameManager.Instance.SetGameState(GameState.InCombat);

        }
    }
}
