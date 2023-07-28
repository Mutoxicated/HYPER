using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats : MonoBehaviour
{
    public Dictionary<string, float[]> incrementalStat = new Dictionary<string, float[]>();
    public Dictionary<string, float[]> decrementalStat = new Dictionary<string, float[]>();

    private List<string> modifiedDecrementals = new List<string>();
    private List<string> modifiedIncrementals = new List<string>();

    //float array contains {value, time, duration in their respective places
    private float[] defaultSet = new float[] { 1f, 0f, 0f };

    private void Awake()
    {
        incrementalStat.Add("moveSpeed", defaultSet)
        ; incrementalStat.Add("damage", defaultSet)
        ; incrementalStat.Add("rate", defaultSet)
        ; incrementalStat.Add("attackSpeed", defaultSet)
        ; incrementalStat.Add("shootSpeed", defaultSet)
        ; 
        //HAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAH
    }

    private void Update()
    {
        CheckIncrementals();
        CheckDecrementals();
    }

    private void CheckIncrementals()
    {
        if (modifiedIncrementals.Count == 0)
            return;
        foreach (var stat in modifiedIncrementals)
        {
            incrementalStat[stat][1] += Time.deltaTime;
            if (incrementalStat[stat][1] > incrementalStat[stat][2])
            {
                RevertIncrementalStat(stat);
            }
        }
    }

    private void CheckDecrementals()
    {
        if (modifiedDecrementals.Count == 0)
            return;
        foreach (var stat in modifiedDecrementals)
        {
            decrementalStat[stat][1] += Time.deltaTime;
            if (decrementalStat[stat][1] > decrementalStat[stat][2])
            {
                RevertDecrementalStat(stat);
            }
        }
    }

    public void ModifyAllIncrementalStats(float value, float duration)
    {
        foreach (var stat in incrementalStat)
        {
            incrementalStat[stat.Key][0] = value;
            incrementalStat[stat.Key][2] = duration;
            modifiedIncrementals.Add(stat.Key);
        }
    }

    public void ModifyAllDecrementalStats(float value, float duration)
    {
        foreach (var stat in decrementalStat)
        {
            decrementalStat[stat.Key][0] = value;
            decrementalStat[stat.Key][2] = duration;
            modifiedDecrementals.Add(stat.Key);
        }
    }

    public void ModifyIncrementalStat(string name, float value, float duration)
    {
        incrementalStat[name][0] = value;
        incrementalStat[name][2] = duration;
        modifiedIncrementals.Add(name);
    }

    public void ModifyDecrementalStat(string name, float value, float duration)
    {
        decrementalStat[name][0] = value;
        decrementalStat[name][2] = duration;
        modifiedDecrementals.Add(name);
    }

    public void RevertIncrementalStats()
    {
        foreach (var keyname in modifiedIncrementals)
        {
            incrementalStat[keyname] = defaultSet;
        }
        modifiedIncrementals.Clear();
    }

    public void RevertDecrementalStats()
    {
        foreach (var keyname in modifiedDecrementals)
        {
            incrementalStat[keyname] = defaultSet;
        }
        modifiedDecrementals.Clear();
    }

    public void RevertIncrementalStat(string name)
    {
        incrementalStat[name] = defaultSet;
        modifiedIncrementals.Remove(name);
    }

    public void RevertDecrementalStat(string name)
    {
        decrementalStat[name] = defaultSet;
        modifiedDecrementals.Remove(name);
    }
}
