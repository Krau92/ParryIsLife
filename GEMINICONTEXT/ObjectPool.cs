using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public GameObject GetPrefab() { return prefab; }

    [SerializeField] private int initialSize;
    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        InitializePool();
    }

    //Creating initial pool
    private void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateObject(prefab);
            pool.Enqueue(obj);
        }
    }

    //Creating a new object
    private GameObject CreateObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        PooledObject pooledObject = obj.GetComponent<PooledObject>();

        //Ensuring the PooledObject component is assigned to this pool and not adding duplicates
        if (pooledObject == null)
            pooledObject = obj.AddComponent<PooledObject>();
        
        pooledObject.pool = this;
        obj.SetActive(false);
        return obj;
    }

    //Getting an object from the pool. Important, who invokes this method must activate the object.
    public GameObject GetFromPool()
    {
        GameObject obj;

        if (pool.Count == 0)
            obj = CreateObject(prefab);
        else
            obj = pool.Dequeue();

        return obj;
    }

    //Returning pooled objects to the pool
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
