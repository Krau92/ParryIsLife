using System.Collections.Generic;
using UnityEngine;

public class TestEnemyShooting : MonoBehaviour
{
    [SerializeField] private Vector2 shootOffset;
    public List<ShootingPatternSO> shootingPatterns;
    [SerializeField] private NewTestBullet bulletPrefab;
    [SerializeField] private Transform[] spawnPositions;
    private Vector2 spawnPosition;

    private float shootTimer;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        
    }

    public void SetSpawnPosition(int i)
    {
        spawnPosition = (Vector2)spawnPositions[i].position;
    }

    public void ShootPattern(int i)
    {
        StartCoroutine(shootingPatterns[i].Shoot(spawnPosition, playerTransform.position, bulletPrefab));
    }
}
