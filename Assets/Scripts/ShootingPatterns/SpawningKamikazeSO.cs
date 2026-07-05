using UnityEngine;

[CreateAssetMenu(fileName = "SpawnKamikaze", menuName = "Scriptable Objects/Spawn Kamikaze")]
public class SpawningKamikazeSO : ShootingPatternSO
{
    public KamikazeEnemy kamikazePrefab;
    public float waitTime;
    public float initialWaitTime;
    public float spreadWidth;
    public float heightOffset;

    protected override void ExecuteShootingPattern(Vector2 shootOrigin, Vector2 objectivePos, NewTestBullet prefabBullet)
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            KamikazeEnemy kamikaze = PoolManager.SpawnObject(kamikazePrefab, shootOrigin, Quaternion.identity, PoolManager.PoolType.Enemy);
            float adjustedWaitTime = initialWaitTime +  waitTime * i;
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            Vector2 position = new Vector2(shootOrigin.x - spreadWidth / 2 + (spreadWidth / (bulletAmount - 1)) * i, shootOrigin.y - heightOffset);
            kamikaze.SetKamikaze(shootOrigin, position, adjustedWaitTime, waitTime, bulletSpeed, player);
        }
    }

}