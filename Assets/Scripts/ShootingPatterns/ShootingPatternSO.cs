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
    
    //Coroutine to handle shooting bursts
    
    public IEnumerator Shoot(Vector2 shootOrigin, Vector2 objectivePos, GenericObjectPool<TestBullet> bulletPool)
    {
        for (int burst = 0; burst < numberOfBursts; burst++)
        {
            ExecuteShootingPattern(shootOrigin, objectivePos, bulletPool);
            yield return new WaitForSeconds(burstCooldown);
        }
    }

    
    //Abstract method to be implemented by child classes for shooting behavior
    protected abstract void ExecuteShootingPattern(Vector2 shootOrigin, Vector2 objectivePos, GenericObjectPool<TestBullet> bulletPool);
}
