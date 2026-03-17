# 2D Boss Rush - Shmup Prototype

![Gameplay Demo](Readme_Images/Shmup.gif)

## Overview
A classic 2D bullet hell Shmup converted into a boss rush, developed in **Unity (C#)**. As this is a more recent project, I was able to write cleaner code applying what I am currently learning in my Master's degree. I took special care to adhere to the *Single Responsibility Principle (SRP)*, splitting the code into single-responsibility classes. This is a single-boss prototype designed to be easily scalable into a full game. Currently, it serves as a short, playable demo.

## Technical decisions
As an Industrial Engineer transitioning to Game Development, I prioritize system architecture. Because I also love game design and want to make the game structure accessible to designers, I created easily configurable shooting patterns. I also ensured that boss behavior could be customized using adjustable wait times between patterns and combos.

### **Data-Driven Design (Scriptable Objects):** 
Bullet patterns (`ConePattern`, `CircularPattern`) and Boss Stats are heavily decoupled from the logic. Game Designers can create complex new bullet hell patterns directly from the Unity Editor without needing any coding knowledge. This also makes in-editor configuration much easier: game designers can simply drag and drop the shooting pattern and configure the waiting times. These objects use inheritance and polymorphism to ensure the game loop can execute any quantity and type of shooting pattern.

**PARENT CLASS**

```csharp
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
        WaitForSeconds wait = new WaitForSeconds(burstCooldown);
        for (int burst = 0; burst < numberOfBursts; burst++)
        {
            ExecuteShootingPattern(shootOrigin, objectivePos, prefabBullet);
            yield return wait;
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
```

**CHILD CLASS EXAMPLE**
```csharp
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
            NewTestBullet bullet = PoolManager.SpawnObject(prefabBullet, shootOrigin, Quaternion.identity, PoolManager.PoolType.Bullets);
            float bulletDirXPosition = shootOrigin.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirYPosition = shootOrigin.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector2 bulletDirection = new Vector2(bulletDirXPosition, bulletDirYPosition) - shootOrigin;
            bulletDirection.Normalize();
            
            SetBulletParameters(bullet, shootOrigin, bulletDirection);
            
            angle += angleStep;
        }
    }
}
```

### **Component-Based Architecture (SRP):** 
As mentioned before, I wanted to apply *clean code* principles to make the codebase more readable, maintainable, and scalable. For example, the Player behavior is divided into single-responsibility components: `PlayerMovement`, `PlayerShoot`, `PlayerParry`, `PlayerMelee`, and `PlayerHealth`.

### **Observer Pattern (Events):**
Improving upon my very first personal prototype, I wanted to decouple my code. To do so, I used `Action` and events (e.g., `CombatEvents.OnBossDamaged`) to separate UI and audio from the core combat logic.
**THE EVENT MANAGER**
```csharp
using UnityEngine;
using System;

public static class CombatEvents
{
    public static Action<SoundEffectSO> OnSoundShouldTrigger;
    public static Action OnChargingParryStart;
    public static Action OnChargingParryEnd;
    public static Action OnParryStart;
    public static Action OnParryEnd;
    public static Action OnReflectingStart;
    public static Action OnReflectingEnd;
    public static Action OnChargingShotStart;
    public static Action OnChargingShotEnd;
    public static Action<bool> OnPlayerShoot;

    
    public static Action<Boss> OnBossSelected;

    public static Action OnDamageTaken;
    public static Action OnPlayerDeath;
    public static Action OnBossDamaged;
    public static Action OnBossDefeated;

    public static Action OnCombatEnded;
    
    public static Action OnParriedBullet;
    public static Action OnReflectedBullet;
    public static Action<int> OnMeleeHit;
    public static Action OnEnemyStunned;

}
```

* **Modern Object Pooling:** Implemented Unity's native `UnityEngine.Pool.ObjectPool` API to handle massive amounts of bullets efficiently without garbage collection spikes. It is used to recycle bullets and `AudioSource` components when an SFX is triggered. An example of this can be seen in the `CircularPatternSO` script above.


## Future improvements
Since this is still a learning project, there are architectural decisions I would approach differently today:
* **Singleton Coupling:** There is still some reliance on a global `GameManager.Instance`. In a production environment, I would replace this to improve testability.
* **Magic Strings:** Some animations and `Invoke` methods use hardcoded strings. I would refactor these for type safety and better performance.