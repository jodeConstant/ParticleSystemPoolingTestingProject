using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// based on example in https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1.html
[System.Serializable]
public class ParticleSystemObjectPool
{
    [SerializeField] GameObject prefab;
    [SerializeField] Transform poolObjectContainer;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = false;
    public int minPoolSize = 51;
    public int maxPoolSize = 70;

    ObjectPool<GameObject> pool;

    public ObjectPool<GameObject> Pool
    {
        get { return pool; }
    }

    public void InitializePool()
    {
        pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, minPoolSize, maxPoolSize);
    }

    GameObject CreatePooledItem()
    {
        if (pool.CountAll > minPoolSize) Debug.LogWarning("Extra item created!");
        GameObject newObject = GameObject.Instantiate(prefab, poolObjectContainer);
        newObject.GetComponent<PoolableParticleSystem>().pool = pool;
        return newObject;
    }

    // Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject systemObject)
    {
        systemObject.SetActive(false);
    }

    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject systemObject)
    {
        systemObject.SetActive(true);
    }

    // If the pool capacity is reached then any items returned will be destroyed.
    void OnDestroyPoolObject(GameObject systemObject)
    {
        GameObject.Destroy(systemObject);
    }
}
