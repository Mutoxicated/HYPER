using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (var stat in modifiedIncrementals.ToArray())
        {
            incrementalStat[stat][1] += Time.deltaTime;
            if (incrementalStat[stat][1] > incrementalStat[stat][2])
            {
                Debug.Log("reverted stat: "+stat);
                RevertIncrementalStat(stat);
            }
        }
    }

    private void CheckDecrementals()
    {
        if (modifiedDecrementals.Count == 0)
            return;
        foreach (var stat in modifiedDecrementals.ToArray())
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
            incrementalStat[stat.Key][0] += value;
            if (incrementalStat[stat.Key][2] < duration)
            {
                incrementalStat[stat.Key][2] = duration;
            }
            modifiedIncrementals.Add(stat.Key);
        }
    }

    public void ModifyAllDecrementalStats(float value, float duration)
    {
        foreach (var stat in decrementalStat)
        {
            decrementalStat[stat.Key][0] += value;
            if (decrementalStat[stat.Key][2] < duration)
            {
                decrementalStat[stat.Key][2] = duration;
            }
            modifiedDecrementals.Add(stat.Key);
        }
    }

    public void ModifyIncrementalStat(string name, float value, float duration)
    {
        incrementalStat[name][0] += value;
        if (incrementalStat[name][2] < duration)
        {
            incrementalStat[name][2] = duration;
            Debug.Log("duration set to: " + duration);
        }
        modifiedIncrementals.Add(name);
    }

    public void ModifyDecrementalStat(string name, float value, float duration)
    {
        decrementalStat[name][0] += value;
        if(decrementalStat[name][2] < duration)
        {
            decrementalStat[name][2] = duration;
        }
        modifiedDecrementals.Add(name);
    }

    public void RevertIncrementalStats()
    {
        foreach (var name in modifiedIncrementals)
        {
            incrementalStat[name][0] = 1f;
            incrementalStat[name][1] = 0f;
            incrementalStat[name][2] = 0f;
        }
        modifiedIncrementals.Clear();
    }

    public void RevertDecrementalStats()
    {
        foreach (var name in modifiedDecrementals)
        {
            decrementalStat[name][0] = 1f;
            decrementalStat[name][1] = 0f;
            decrementalStat[name][2] = 0f;
        }
        modifiedDecrementals.Clear();
    }

    public void RevertIncrementalStat(string name)
    {
        incrementalStat[name][0] = 1f;
        incrementalStat[name][1] = 0f;
        incrementalStat[name][2] = 0f;
        Debug.Log(incrementalStat[name][0] + " / "+ incrementalStat[name][1] + " / " + incrementalStat[name][2]);
        modifiedIncrementals.Remove(name);
    }

    public void RevertDecrementalStat(string name)
    {
        decrementalStat[name][0] = 1f;
        decrementalStat[name][1] = 0f;
        decrementalStat[name][2] = 0f;
        modifiedDecrementals.Remove(name);
    }
}
