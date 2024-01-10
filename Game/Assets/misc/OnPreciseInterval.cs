using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class OnPreciseInterval : MonoBehaviour
{
    [SerializeField] private string specialTag = "Untagged";
    [SerializeField, Range(0, 100)] private int chance = 100;
    [SerializeField] private int intervalMilliseconds;
    [SerializeField] public UnityEvent onInterval;

    private Stopwatch stopwatch = new Stopwatch();

    [HideInInspector] public float t = 0f;
    [HideInInspector] public bool isPlaying;

    public void ResetEarly()//this is stupid but oh well lol
    {
        ResetInterval();
    }

    public void ResetEventless()
    {
        stopwatch.Reset();
        t = 0f;
    }

    private void ResetInterval()
    {
        if (Random.Range(0, 100) <= chance)
            onInterval.Invoke();
        stopwatch.Reset();
        t = 0f;
    }

    public void Pause()
    {
        isPlaying = false;
        stopwatch.Stop();
    }

    public void Resume()
    {
        isPlaying = true;
        stopwatch.Start();
    }

    public void ManipulateT(float t)
    {
        this.t = t;
    }

    public void ChangeInterval(int intervalMilliseconds)
    {
        this.intervalMilliseconds = intervalMilliseconds;
    }

    private void OnEnable()
    {
        t = 0f;
        isPlaying = true;
        stopwatch.Reset();
        stopwatch.Start();
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;
        if (!isPlaying)
            return;
        //Debug.Log(time);
        if (stopwatch.ElapsedMilliseconds >= intervalMilliseconds)
        {
            ResetInterval();
        }
        if (stopwatch.ElapsedMilliseconds != 0f)
            t = stopwatch.ElapsedMilliseconds / intervalMilliseconds;
        else t = 0f;
    }
}
