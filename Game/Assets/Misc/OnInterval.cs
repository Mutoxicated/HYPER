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
    [SerializeField] private UnityEvent onInterval;
    [SerializeField] private bool destroyUponEvent;

    private bool useStats = false;
    [HideInInspector] public float t;
    private float time;

    public void ResetEarly()
    {
        ResetInterval();
    }

    private void ResetInterval()
    {
        time = 0f;
        t= 0f;
        if (Random.Range(0, 100) <= chance)
            onInterval.Invoke();
        if (destroyUponEvent)
            Destroy(gameObject);
    }

    private void Awake()
    {
        if (stats != null)
            useStats = true;
    }

    private void Update()
    {
        if (useStats)
            time += Time.deltaTime * stats.incrementalStat["attackSpeed"];
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
