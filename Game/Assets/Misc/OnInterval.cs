using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnInterval : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private string specialTag = "Untagged";
    [SerializeField, Range(0, 100)] private int chance = 100;
    [SerializeField] private float interval;
    [SerializeField] public UnityEvent onInterval;
    [SerializeField] private bool destroyUponEvent;
    [SerializeField] private bool recycleUponEvent;

    private bool useStats = false;
    [HideInInspector] public float t = 0f;
    [HideInInspector]
    public float time = 0f;
    [HideInInspector] public bool isPlaying;

    public void ResetEarly()//this is stupid but oh well lol
    {
        ResetInterval();
    }

    public void ResetEventless()
    {
        time = 0f;
        t = 0f;
    }

    private void ResetInterval()
    {
        if (Random.Range(0, 100) <= chance)
            onInterval.Invoke();
        if (destroyUponEvent)
            Destroy(gameObject);
        if (recycleUponEvent)
            PublicPools.pools[gameObject.name].Reattach(gameObject);
        time = 0f;
        t = 0f;
    }

    public void Pause()
    {
        isPlaying = false;
    }

    public void Resume()
    {
        isPlaying = true;
    }

    private void OnEnable()
    {
        time = 0f;
        t = 0f;
        isPlaying = true;
        if (stats != null)
            useStats = true;
    }

    private void Update()
    {
        if (!isPlaying)
            return;
        if (useStats)
            time += Time.deltaTime * stats.incrementalStat["rate"][0];
        else
            time += Time.deltaTime;
        //Debug.Log(time);
        if (time >= interval)
        {
            ResetInterval();
        }
        if (time != 0f)
            t = time / interval;
        else t = 0f;
    }
}
