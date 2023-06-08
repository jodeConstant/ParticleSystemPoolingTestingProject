using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeAllocationSimulator : MonoBehaviour
{
    public int allocationSize;
    int[] allocationChunk;

    public float allocationInterval = 2f;
    float timer;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            allocationChunk = new int[allocationSize];
            timer += allocationInterval;
        }
    }
}
