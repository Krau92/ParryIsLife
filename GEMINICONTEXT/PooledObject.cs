using UnityEngine;

/*
    Poolable object class to be attached to objects managed by ObjectPool.
    Examples: bullets, enemies, projectiles, VFX, SFX.
    NOT TO INCLUDE: Bosses.
*/
public class PooledObject : MonoBehaviour
{
    public ObjectPool pool;

    public void ReturnToPool()
    {
        if (pool != null)
        {
            pool.ReturnToPool(this.gameObject);
            return;
        }
        //In case no pool is assigned, or dissapears, destroy the object
        Destroy(this.gameObject);
        Debug.Log("PooledObject: No pool assigned, destroying object.");
    }

}
