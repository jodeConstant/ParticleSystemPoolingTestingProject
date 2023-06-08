using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[System.Serializable]
public class ObjectPool
{
    [SerializeField] GameObject[] objects;
    int index = 0;

    private bool PrepareNext()
    {
        int nextObjectIndex = index + 1;
        if (nextObjectIndex >= objects.Length) nextObjectIndex = 0;
        if (objects[nextObjectIndex].activeSelf)
        {
            objects[nextObjectIndex].SetActive(false);
            Debug.LogWarning("An upcoming ParticleSystem object " + nextObjectIndex + " was still active!");
            return true;
        }
        return false;
    }

    public GameObject next
    {
        get
        {
            index++;
            if (index >= objects.Length) { index = 0; }
            PrepareNext();
            return objects[index];
        }
    }
}
*/

public class ParticleSystemPool : MonoBehaviour, IParticleRequestHandler
{
    /* Just for the sake of functional implementation */
    public ParticleSystemObjectPool[] pools;

    void Start()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].InitializePool();
        }
    }

    public void SpawnEffectAtPosition(ParticleEffect type, Vector3 position, Transform relativeTo = null)
    {
        // guard clauses
        if (pools == null) return;
        
        if (((int)type >= pools.Length) || ((int)type < 0))
        {
            Debug.LogError("Invalid type for particle effect request");
        }

        GameObject particleSystem = pools[(int)type].Pool.Get();
        particleSystem.transform.position = (relativeTo == null) ? position : relativeTo.TransformPoint(position);
        //particleSystem.SetActive(true);
    }
}
