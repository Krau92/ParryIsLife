using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{

    [Header("Pool Settings")]
    [SerializeField] protected T prefab; // Generic prefab to pool
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 2000;
    [SerializeField] private bool collectionCheck = false;

    private IObjectPool<T> pool;

    protected void InitializePool()
    {
        pool = new ObjectPool<T>(
            createFunc: CreateItem,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: collectionCheck, // False para rendimiento óptimo
            defaultCapacity: defaultCapacity,
            maxSize: maxPoolSize
        );
    }
    protected virtual void AssignPoolToItem(T item)
    {
        // Assign the pool reference to the pooled item if needed
    }

    protected virtual T CreateItem()
    {
        // Instantiate prefab
        T instance = Instantiate(prefab);
        instance.transform.position = Camera.main.transform.position + Vector3.back * 1000;
        instance.gameObject.SetActive(false);
        AssignPoolToItem(instance);
        return instance;
    }

    protected virtual void OnTakeFromPool(T item)
    {
        item.gameObject.SetActive(true);
    }

    protected virtual void OnReturnToPool(T item)
    {
        item.transform.position = Camera.main.transform.position + Vector3.back * 1000;
        item.gameObject.SetActive(false);
    }

    protected virtual void OnDestroyPoolObject(T item)
    {
        Destroy(item.gameObject);
    }

    public T Get()
    {
        return pool.Get();
    }

    public void ReturnToPool(T item)
    {
        pool.Release(item);
    }
}