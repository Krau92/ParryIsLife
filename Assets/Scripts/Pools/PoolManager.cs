using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private bool dontDestroyOnLoad = false; //If true, the created Pool will persist across scenes

    private static GameObject emptyHolder;

    //!Add here all the static references to the different pools
    private static GameObject bulletsSystemEmpty;
    private static GameObject gameObjectsSystemEmpty;

    //Dictionary to find pools by prefab
    private static Dictionary<GameObject, ObjectPool<GameObject>> objectPools;
    private static Dictionary<GameObject, GameObject> cloneToPrefabMap;

    public enum PoolType
    {
        Bullets,    //player and enemy Bullets
        GameObjects //Generic GameObjects

        //!Add more pool types here
    }
    public static PoolType poolType;

    private void Awake()
    {
        //Instantiate here because we activate DO NOT RELOAD DOMAIN OR SCENE on Enter Play Mode Settings
        objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

        SetupEmpties();
    }

    //Creating the hierarchy for the pools and adding the DontDestroyOnLoad if needed
    private void SetupEmpties()
    {
        emptyHolder = new GameObject("Object Pools");

        bulletsSystemEmpty = new GameObject("Bullets Pool");
        bulletsSystemEmpty.transform.SetParent(emptyHolder.transform);

        gameObjectsSystemEmpty = new GameObject("GameObjects Pool");
        gameObjectsSystemEmpty.transform.SetParent(emptyHolder.transform);

        //!Add more empties here

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(bulletsSystemEmpty.transform.root);
        }
    }

    private static void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType type = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );

        objectPools.Add(prefab, pool);
    }

    private static void CreatePool(GameObject prefab, Transform parent, Quaternion rot, PoolType type = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, parent, rot),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );

        objectPools.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType type = PoolType.GameObjects)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, pos, rot);

        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(type);
        obj.transform.SetParent(parentObject.transform);

        return obj;

    }

    private static GameObject CreateObject(GameObject prefab, Transform parent, Quaternion rot, PoolType type = PoolType.GameObjects)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, parent);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = rot;
        obj.transform.localScale = Vector3.one;

        prefab.SetActive(true);

        return obj;

    }

    private static void OnGetObject(GameObject obj)
    {
        //Additional logic when getting an object from the pool can be added here
    }

    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private static void OnDestroyObject(GameObject obj)
    {
        if(cloneToPrefabMap.ContainsKey(obj))
        {
            cloneToPrefabMap.Remove(obj);
        }
    }

    private static GameObject SetParentObject(PoolType type)
    {
        switch (type)
        {
            case PoolType.Bullets:
                return bulletsSystemEmpty;
            case PoolType.GameObjects:
                return gameObjectsSystemEmpty;


            //!Add more cases here
            default:
                return null;
        }
    }

   
    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 pos, Quaternion rot, PoolType type = PoolType.GameObjects) where T : Object
    {
        if(!objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, pos, rot, type);
        }

        GameObject obj = objectPools[objectToSpawn].Get();

        if(obj != null)
        {
            if(!cloneToPrefabMap.ContainsKey(obj))
            {
                cloneToPrefabMap.Add(obj, objectToSpawn);
            }
        

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);

            if(typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }
            
            T component = obj.GetComponent<T>();

            if(component == null)
            {
                Debug.LogWarning("The spawned object does not have the requested component of type: " + typeof(T).ToString());
                return null;
            }

            return component;
        }

        return null;
    }

    //To spawn components from the pools
    public static T SpawnObject<T>(T prefab, Vector3 pos, Quaternion rot, PoolType type = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(prefab.gameObject, pos, rot, type);
    }

    public static GameObject SpawnObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType type = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(prefab, pos, rot, type);
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Transform parent, Quaternion rot, PoolType type = PoolType.GameObjects) where T : Object
    {
        if(!objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, parent, rot, type);
        }

        GameObject obj = objectPools[objectToSpawn].Get();

        if(obj != null)
        {
            if(!cloneToPrefabMap.ContainsKey(obj))
            {
                cloneToPrefabMap.Add(obj, objectToSpawn);
            }
        

            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = rot;
            obj.SetActive(true);

            if(typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }
            
            T component = obj.GetComponent<T>();

            if(component == null)
            {
                Debug.LogWarning("The spawned object does not have the requested component of type: " + typeof(T).ToString());
                return null;
            }

            return component;
        }

        return null;
    }

    public static T SpawnObject<T>(T prefab, Transform parent, Quaternion rot, PoolType type = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(prefab.gameObject, parent, rot, type);
    }

    public static GameObject SpawnObject(GameObject prefab, Transform parent, Quaternion rot, PoolType type = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(prefab, parent, rot, type);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType type = PoolType.GameObjects)
    {
        if(cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(type);

            if(obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }

            if(objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Trying to return an object that is not pooled");
        }
        
    }

}


