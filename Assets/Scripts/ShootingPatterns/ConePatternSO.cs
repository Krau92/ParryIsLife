using UnityEngine;

[CreateAssetMenu(fileName = "ConePattern", menuName = "Scriptable Objects/ConePattern")]
public class ConePatternSO : ShootingPatternSO
{
    public float coneAngle = 45f;
    protected override void ExecuteShootingPattern(Vector2 shootOrigin, Vector2 objectivePos, GenericObjectPool<TestBullet> bulletPool)
{
    Vector2 direction = objectivePos - shootOrigin;
    float initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    float startAngle = initialAngle - (coneAngle / 2f);
    
    float angleStep = coneAngle / (bulletAmount - 1) ;

    for (int i = 0; i < bulletAmount; i++)
    {
        
        TestBullet bullet = bulletPool.Get();
        float currentAngle = startAngle + (angleStep * i);
        
        if (bulletAmount == 1) currentAngle = initialAngle;

        float radians = currentAngle * Mathf.Deg2Rad;
        Vector2 bulletDirection = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));


            bullet.SetDirection(bulletDirection);


            //!Can this go in the parent class in order to DRY?
            bullet.transform.position = shootOrigin;
            bullet.SetOrigin(shootOrigin);
            bullet.SetSpeed(bulletSpeed);
            bullet.SetCircularSpeed(rotationSpeed);
        
        // Visually rotate the bullet to match its direction
        bullet.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
}
