using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "RandomPattern", menuName = "Scriptable Objects/RandomPattern")]
public class RandomPatternSO : ShootingPatternSO
{
    [Header("Random Ranges")]
    public Vector2Int randomBursts = new Vector2Int { x = 1, y = 5 };
    public Vector2 randomAngle = new Vector2 { x = 0f, y = 360f };
    public Vector2 randomRadius = new Vector2 { x = 0f, y = 2f };
    public Vector2Int randomBulletPerBurst = new Vector2Int { x = 1, y = 10 };
    public Vector2 randomSpeed = new Vector2 { x = 1f, y = 10f };
    public float parreableChance = 0.5f;

    public override IEnumerator Shoot(Vector2 shootOrigin, Vector2 objectivePos, NewTestBullet prefabBullet)
    {
        numberOfBursts = Random.Range(randomBursts.x, randomBursts.y + 1);
        return base.Shoot(shootOrigin, objectivePos, prefabBullet);
    }

    protected override void ExecuteShootingPattern(Vector2 shootOrigin, Vector2 objectivePos, NewTestBullet prefabBullet)
    {
        bulletAmount = Random.Range(randomBulletPerBurst.x, randomBulletPerBurst.y + 1);
        bulletSpeed = Random.Range(randomSpeed.x, randomSpeed.y);


        for (int i = 0; i < bulletAmount; i++)
        {
            bool isParreable = Random.value < parreableChance;
            float angle = Random.Range(randomAngle.x, randomAngle.y);
            float radians = angle * Mathf.Deg2Rad;
            Vector2 bulletDirection = new Vector2(Mathf.Sin(radians), -Mathf.Cos(radians));

            float radius = Random.Range(randomRadius.x, randomRadius.y);
            Vector2 spawnPos = shootOrigin + bulletDirection * radius;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

            NewTestBullet bullet = PoolManager.SpawnObject(prefabBullet, spawnPos, bulletRotation, PoolManager.PoolType.Bullets);
            SetBulletParameters(bullet, spawnPos, bulletDirection, isParreable);
        }
    }
}
