using UnityEngine;
using System.Collections;

//Abstract parent class for shooting patterns
public abstract class ShootingPatternSO : ScriptableObject
{
    public float bulletSpeed;
    public float rotationSpeed;
    public int bulletAmount;
    public int numberOfBursts;
    public float burstCooldown;
    public bool IsParreable;
    public NewTestBullet prefabBullet;

    //Coroutine to handle shooting bursts

    public IEnumerator Shoot(Vector2 shootOrigin, Vector2 objectivePos, NewTestBullet prefabBullet)
    {
        for (int burst = 0; burst < numberOfBursts; burst++)
        {
            ExecuteShootingPattern(shootOrigin, objectivePos, prefabBullet);
            yield return new WaitForSeconds(burstCooldown);
        }
    }

    //Sets the shooting pattern parameters and assign the owner layer of the bullet
    protected void SetBulletParameters(NewTestBullet bullet, Vector2 shootOrigin, Vector2 bulletDirection)
    {
        bullet.ConfigureBullet(bulletDirection, bulletSpeed, true, IsParreable, shootOrigin);
        bullet.SetCircularSpeed(rotationSpeed);
        

    }

    protected void SetBulletParameters(NewTestBullet bullet, Vector2 bulletDirection)
    {
        bullet.ConfigureBullet(bulletDirection, bulletSpeed, true, IsParreable);
        bullet.SetCircularSpeed(rotationSpeed);
    }


    //Abstract method to be implemented by child classes for shooting behavior
    protected abstract void ExecuteShootingPattern(Vector2 shootOrigin, Vector2 objectivePos, NewTestBullet prefabBullet);
}
