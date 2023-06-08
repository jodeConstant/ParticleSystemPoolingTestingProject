using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Just for the sake of functional implementation
    Multiple types are not needed for performance comparison
*/
public enum ParticleEffect
{
    Bubbles
}

public class ParticleSpawnRequester : MonoBehaviour
{
    public ParticleSystemPool pool;
    public ParticleSystemLibrary library;

    IParticleRequestHandler requestHandler;

    public Transform[] markers;
    public int markerIndex = 0;
    public float duration = 15f;
    float mainTimer;
    public float startDelay = 0.5f;
    public float cooldown;
    float timer;
    public int objectsAtATime = 1;
    int objects_i;

    void Start()
    {
        if (pool != null) requestHandler = pool;
        else if (library != null) requestHandler = library;
        else enabled = false;

        timer = startDelay;
        mainTimer = duration;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        mainTimer -= Time.deltaTime;

        // heavy computing to make sure memory management doesn't have excessive time
        for (int s = 0; s < 200000; s++)
        {
            Mathf.Sqrt(s);
        }

        if (timer <= 0f)
        {
            for (objects_i = 0; objects_i < objectsAtATime; objects_i++)
            {
                requestHandler.SpawnEffectAtPosition(ParticleEffect.Bubbles, markers[markerIndex].position);
            }
            timer = cooldown;
            markerIndex++;
            if (markerIndex >= markers.Length) { markerIndex = 0; }
        }

        if (mainTimer <= 0f)
        {
            enabled = false;
        }
    }
}
