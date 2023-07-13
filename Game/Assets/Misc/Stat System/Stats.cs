using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats : MonoBehaviour
{
    public Dictionary<string,float> incrementalStat = new Dictionary<string,float>();
    public Dictionary<string, float> decrementalStat = new Dictionary<string, float>();

    private List<string> modifiedDecrementals = new List<string>();
    private List<string> modifiedIncrementals = new List<string>();

    private float duration;
    private float t;

    private void Awake()
    {
        incrementalStat.Add("moveSpeed", 1f);
        incrementalStat.Add("damage", 1f);
        incrementalStat.Add("attackSpeed", 1f);
        incrementalStat.Add("precision", 1f);

        decrementalStat.Add("defense", 1f);
    }

    private void Update()
    {
        if (duration <= 0f)
            return;

        t += Time.deltaTime;
        if (t >= duration)
        {
            t = 0f;
            duration = -1f;
            RevertStats();
        }
    }

    public void ModifyAllIncrementalStats(float value, float duration)
    {
        foreach (var buff in incrementalStat)
        {
            incrementalStat[buff.Key] = value;
            modifiedIncrementals.Add(buff.Key);
        }
        this.duration = duration;
    }

    public void ModifyAllDecrementalStats(float value, float duration)
    {
        foreach (var buff in decrementalStat)
        {
            decrementalStat[buff.Key] = value;
            modifiedDecrementals.Add(buff.Key);
        }
        this.duration = duration;
    }

    public void ModifyIncrementalStat(string name, float value, float duration)
    {
        incrementalStat[name] = value;
        modifiedIncrementals.Add(name);
        this.duration = duration;
    }

    public void ModifyDecrementalStat(string name, float value, float duration)
    {
        incrementalStat[name] = value;
        modifiedDecrementals.Add(name);
        this.duration = duration;
    }

    public void RevertStats()
    {
        foreach (var keyname in modifiedIncrementals)
        {
            incrementalStat[keyname] = 1f;
        }
        modifiedIncrementals.Clear();
        foreach (var keyname in modifiedDecrementals)
        {
            decrementalStat[keyname] = 1f;
        }
        modifiedDecrementals.Clear();
    }

    public void RevertIncrementalStat(string name)
    {
        incrementalStat[name] = 1f;
        modifiedIncrementals.Remove(name);
    }

    public void RevertDecrementalStat(string name)
    {
        decrementalStat[name] = 1f;
        modifiedDecrementals.Remove(name);
    }
}
