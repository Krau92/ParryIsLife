using UnityEngine;
using UnityEngine.Pool;

public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
{
    public static GenericObjectPool<T> Instance { get; private set; }

    [Header("Pool Settings")]
    [SerializeField] protected T prefab; // Generic prefab to pool
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 2000;
    [SerializeField] private bool collectionCheck = false;

    private IObjectPool<T> pool;

    protected virtual void Awake()
    {
        // Singleton Genérico
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializePool();
    }


    private void InitializePool()
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


    protected virtual T CreateItem()
    {
        // Instantiate prefab
        T instance = Instantiate(prefab);
        return instance;
    }

    protected virtual void OnTakeFromPool(T item)
    {
        item.gameObject.SetActive(true);
    }

    protected virtual void OnReturnToPool(T item)
    {
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