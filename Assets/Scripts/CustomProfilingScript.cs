using System.IO;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Profiling;

public class CustomProfilingScript : MonoBehaviour
{
    uint[][] memoryUseObjects;// allocated, including non-collected
    uint[][] memoryUseReserved;// Unity's reserved memory
    ushort[][] objectCount;
    double[][] frameTimes;
    float[][] timeElapsed;

    Queue<double> frameTimesQueue;
    FrameTiming[] frameTimings;
    ulong frameCount = 0;
    
    public float countDown = 0f;// allows starting profiling after a delay
    
    public int iSize, jSize;
    int i_index, j_index;
    long memoryClipValue = uint.MaxValue;
    long memoryReading;

    public ParticleSpawnRequester requester;
    public Transform objectContainer;

    string filePath;

    void Awake()
    {
        filePath = string.Format(
                "{0}/ProfilingDataMemoryAndFrames_{1}.bin", 
                Application.persistentDataPath, 
                DateTime.Now.ToString("yyyyMMdd_hhmmss")
                );

        AllocateLogArrays();

        frameTimesQueue = new Queue<double>(5);

        frameTimings = new FrameTiming[1];
    }

    void AllocateLogArrays()
    {
        int i;

        memoryUseObjects = new uint[iSize][];
        for (i = 0; i < iSize; i++)
        {
            memoryUseObjects[i] = new uint[jSize];
        }

        memoryUseReserved = new uint[iSize][];
        for (i = 0; i < iSize; i++)
        {
            memoryUseReserved[i] = new uint[jSize];
        }

        objectCount = new ushort[iSize][];
        for (i = 0; i < iSize; i++)
        {
            objectCount[i] = new ushort[jSize];
        }
        
        frameTimes = new double[iSize][];
        for (i = 0; i < iSize; i++)
        {
            frameTimes[i] = new double[jSize];
        }
        
        timeElapsed = new float[iSize][];
        for (i = 0; i < iSize; i++)
        {
            timeElapsed[i] = new float[jSize];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown > 0f)
        {
            countDown -= Time.deltaTime;
            return;
        }

        FrameTimingManager.CaptureFrameTimings();
        frameCount++;
        
        if ((frameCount > 2ul) && (FrameTimingManager.GetLatestTimings(1, frameTimings) > 0))
        {
            frameTimesQueue.Enqueue(frameTimings[0].cpuMainThreadFrameTime);
        }
        else
        {
            frameTimesQueue.Enqueue(-1d);// for first
        }

        memoryReading = Profiler.GetMonoUsedSizeLong();
        memoryUseObjects[i_index][j_index] = (memoryReading < memoryClipValue) ? (uint)memoryReading : 0;

        memoryReading = Profiler.GetTotalReservedMemoryLong();
        memoryUseReserved[i_index][j_index] = (memoryReading < memoryClipValue) ? (uint)memoryReading : 0;

        objectCount[i_index][j_index] = (ushort)objectContainer.childCount;

        frameTimes[i_index][j_index] = frameTimesQueue.Dequeue();

        timeElapsed[i_index][j_index] = Time.realtimeSinceStartup;

        j_index++;
        if (j_index >= jSize)// inner array filled
        {
            i_index++;// next in outer dimension
            j_index = 0;// reset inner array index
            if (i_index >= iSize)// whole array filled, disable
            {
                StartCoroutine(WriteRoutine());
                enabled = false;
            }
        }
    }

    IEnumerator WriteRoutine()
    {
        Debug.Log("WriteRoutine started");
        yield return new WaitForSeconds(5f);
        WriteToBIN();
        Debug.Log("WriteRoutine ended");
    }

    void WriteToBIN()
    {
        // write memory & frametimes to a binary file
        using (FileStream stream = File.Open(filePath, FileMode.Create))
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                for (int i = 0; i < iSize; i++)
                {
                    for (int j = 0; j < jSize; j++)
                    {
                        writer.Write(memoryUseObjects[i][j]);// uint
                        writer.Write(memoryUseReserved[i][j]);// uint
                        writer.Write(objectCount[i][j]);// ushort
                        writer.Write(frameTimes[i][j]);// double
                        writer.Write(timeElapsed[i][j]);// float
                    }
                }
            }
        }
        // quit at the end
        Application.Quit();
    }
}
