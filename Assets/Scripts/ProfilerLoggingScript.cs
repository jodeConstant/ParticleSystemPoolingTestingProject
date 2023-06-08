using System;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class ProfilerLoggingScript : MonoBehaviour
{
    public Image indicator;// changes color to show when profiling period is over
    public Text totalFramesDisplay;

    public ParticleSpawnRequester requester;

    public int frameCount = 0;
    public int totalFramesCaptured = 0;

    int logIndex = 0;
    
    string LogFileName
    {
        get
        {
            logIndex++;
            return string.Format(
                "{0}/ProfilerLog_{1}_{2}", 
                Application.persistentDataPath,
                ((requester.pool != null) ? "P" : "R"),
                logIndex
                );
        }
    }

    public float timer = 30f;


    void Start()
    {
        frameCount = 0;
        totalFramesCaptured = 0;

        Profiler.logFile = LogFileName;
        Profiler.enableBinaryLog = true;

        Profiler.maxUsedMemory = 1024 * 1024 * 1024;

        // disable unnecessary parts:
        if (Profiler.GetAreaEnabled(ProfilerArea.GPU))
        {
            Profiler.SetAreaEnabled(ProfilerArea.GPU, false); Debug.Log("GPU");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.Rendering))
        {
            Profiler.SetAreaEnabled(ProfilerArea.Rendering, false); Debug.Log("Rendering");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.Audio))
        {
            Profiler.SetAreaEnabled(ProfilerArea.Audio, false); Debug.Log("Audio");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.Video))
        {
            Profiler.SetAreaEnabled(ProfilerArea.Video, false); Debug.Log("Video");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.Physics))
        {
            Profiler.SetAreaEnabled(ProfilerArea.Physics, false); Debug.Log("Physics");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.Physics2D))
        {
            Profiler.SetAreaEnabled(ProfilerArea.Physics2D, false); Debug.Log("Physics2D");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.NetworkMessages))
        {
            Profiler.SetAreaEnabled(ProfilerArea.NetworkMessages, false); Debug.Log("NetworkMessages");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.NetworkOperations))
        {
            Profiler.SetAreaEnabled(ProfilerArea.NetworkOperations, false); Debug.Log("NetworkOperations");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.UI))
        {
            Profiler.SetAreaEnabled(ProfilerArea.UI, false); Debug.Log("UI");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.UIDetails))
        {
            Profiler.SetAreaEnabled(ProfilerArea.UIDetails, false); Debug.Log("UIDetails");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.GlobalIllumination))
        {
            Profiler.SetAreaEnabled(ProfilerArea.GlobalIllumination, false); Debug.Log("GlobalIllumination");
        }
        if (Profiler.GetAreaEnabled(ProfilerArea.VirtualTexturing))
        {
            Profiler.SetAreaEnabled(ProfilerArea.VirtualTexturing, false); Debug.Log("VirtualTexturing");
        }
        
        Profiler.SetAreaEnabled(ProfilerArea.Memory, true);
        // enables profiler:
        Profiler.SetAreaEnabled(ProfilerArea.CPU, true);
        //Profiler.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        totalFramesCaptured++;
        if (frameCount > 300)
        {
            Profiler.enabled = false;
            //Profiler.logFile = "";
            frameCount = 0;

            Profiler.logFile = LogFileName;
            Profiler.enabled = true;
            Debug.LogWarning("Log file " + logIndex + " done");
        }
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            enabled = false;
            indicator.color = Color.cyan;
        }
    }

    void OnDisable()
    {
        if (Profiler.enabled)
        {
            Profiler.enabled = false;
            Profiler.logFile = "";
        }
        totalFramesDisplay.text = totalFramesCaptured.ToString();
    }

    public void AppQuit() { Application.Quit(); }
}
