using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemLibrary : MonoBehaviour, IParticleRequestHandler
{
    public GameObject[] particleEffectPrefabs;

    /* Just for the sake of functional implementation */
    public void SpawnEffectAtPosition(ParticleEffect type, Vector3 position, Transform relativeTo = null)
    {
        // guard clause
        if (((int)type >= particleEffectPrefabs.Length) || ((int)type < 0))
        {
            Debug.LogError("Invalid type for particle effect request");
        }

        GameObject particleSystem = Instantiate(particleEffectPrefabs[(int)type], transform);
        particleSystem.transform.position = (relativeTo == null) ? position : relativeTo.TransformPoint(position);
        particleSystem.SetActive(true);
    }
}
