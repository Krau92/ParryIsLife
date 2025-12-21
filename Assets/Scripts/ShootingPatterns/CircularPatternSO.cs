using UnityEngine;

[CreateAssetMenu(fileName = "CircularPattern", menuName = "Scriptable Objects/CircularPattern")]
public class CircularPatternSO : ShootingPatternSO
{
protected override void ExecuteShootingPattern(Vector2 shootOrigin, Vector2 objectivePos, GenericObjectPool<TestBullet> bulletPool)
    {
        float angleStep = 360f / bulletAmount;
        float angle = 0f;

        for (int i = 0; i < bulletAmount; i++)
        {
            TestBullet bullet = bulletPool.Get();
            float bulletDirXPosition = shootOrigin.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirYPosition = shootOrigin.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector2 bulletDirection = new Vector2(bulletDirXPosition, bulletDirYPosition) - shootOrigin;
            bulletDirection.Normalize();
            
            SetBulletParameters(bullet, shootOrigin, bulletDirection);
            
            angle += angleStep;
        }
    }
}
