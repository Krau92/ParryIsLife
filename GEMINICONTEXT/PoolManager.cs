using UnityEngine;
using System.Collections.Generic;


public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    private void Awake()
    {
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        InitializePools();
    }

    Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

    void InitializePools()
    {
        //Storing all object pools in the scene on the dictionary
        ObjectPool[] objectPools = FindObjectsByType<ObjectPool>(FindObjectsSortMode.None);
        foreach (ObjectPool pool in objectPools)
        {
            GameObject prefab = pool.GetPrefab();
            //Debugging logs for possible mistakes
            if (prefab == null)
            {
                Debug.LogError($"PoolManager: ObjectPool {pool.name} has no prefab assigned.");
                continue;
            }

            if (pools.ContainsKey(prefab))
            {
                Debug.LogError($"PoolManager: Duplicate ObjectPool for prefab {prefab.name} found in {pool.name}. Only one pool per prefab is allowed.");
                continue;
            }

            //Storing the pool using its prefab as key
            pools.Add(prefab, pool);

        }
    }

    //Getting an object from the pool corresponding to the given prefab
    /// <remarks>AXEL!!! I'm your past me! Remember activates the invoked object after placing it.</remarks>
    public GameObject GetGameObjectFromPool(GameObject prefab)
    {
        if (pools.ContainsKey(prefab))
        {
            return pools[prefab].GetFromPool();
        }
        else
        {
            Debug.LogError("PoolManager: No ObjectPool found for prefab " + prefab.name + ".");
            return null;
        }
    }
}
