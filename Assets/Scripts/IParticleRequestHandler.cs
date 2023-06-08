using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Just for the sake of functional implementation */
public interface IParticleRequestHandler
{
    public void SpawnEffectAtPosition(ParticleEffect type, Vector3 position, Transform relativeTo = null);
}
