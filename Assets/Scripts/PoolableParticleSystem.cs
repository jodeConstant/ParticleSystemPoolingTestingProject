using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// based on example in https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1.html
[RequireComponent(typeof(ParticleSystem))]
public class PoolableParticleSystem : MonoBehaviour
{
    public ObjectPool<GameObject> pool;

    void OnParticleSystemStopped()
    {
        // Return to the pool
        pool.Release(gameObject);
    }
}
