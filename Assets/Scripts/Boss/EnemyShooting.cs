using System.Collections.Generic;
using UnityEngine;


public class EnemyShooting : MonoBehaviour
{
    //TODO: Implementar la lista de patterns de disparo
    [SerializeField] private NewTestBullet bulletPrefab;
    [SerializeField] private float shootCheckInterval = 3f;
    [SerializeField] private List<ShootingPatternSO> shootingPattern;
    [SerializeField] private Transform player;
    
    private float shootTimer;

    void OnEnable()
    {
        shootTimer = 0f;
    }



    void Update()
    {
        //For testing purposes, shoot every interval
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootCheckInterval)
        {
            Vector3 targetCenter = player.GetComponent<SpriteRenderer>().bounds.center;
            foreach (var pattern in shootingPattern)
            {
                StartCoroutine(pattern.Shoot(transform.position, targetCenter, bulletPrefab));
            }
            shootTimer = 0f;
        }
    }
}
