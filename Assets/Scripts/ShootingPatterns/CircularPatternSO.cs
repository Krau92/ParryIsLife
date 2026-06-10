using UnityEngine;

[CreateAssetMenu(fileName = "CircularPattern", menuName = "Scriptable Objects/CircularPattern")]
public class CircularPatternSO : ShootingPatternSO
{
protected override void ExecuteShootingPattern(Vector2 shootOrigin, Vector2 objectivePos, NewTestBullet prefabBullet)
    {
        float angleStep = 360f / bulletAmount;
        float angle = 0f;

        for (int i = 0; i < bulletAmount; i++)
        {
            
            float bulletDirXPosition = shootOrigin.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirYPosition = shootOrigin.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector2 bulletDirection = new Vector2(bulletDirXPosition, bulletDirYPosition) - shootOrigin;
            bulletDirection.Normalize();
            Quaternion bulletRotation = Quaternion.FromToRotation(Vector2.up, bulletDirection);
            NewTestBullet bullet = PoolManager.SpawnObject(prefabBullet, shootOrigin, bulletRotation, PoolManager.PoolType.Bullets);
            
            SetBulletParameters(bullet, shootOrigin, bulletDirection);

            
            angle += angleStep;
        }
    }
}
