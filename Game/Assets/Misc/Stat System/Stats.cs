using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

[Serializable]
public class Stats : MonoBehaviour
{
    public Dictionary<string, bool> conditionalStat = new Dictionary<string, bool>();

    public Dictionary<string, float[]> incrementalStat = new Dictionary<string, float[]>();
    public Dictionary<string, float[]> decrementalStat = new Dictionary<string, float[]>();

    [HideInInspector] public List<string> modifiedDecrementals = new List<string>();
    [HideInInspector] public List<string> modifiedIncrementals = new List<string>();

    [Tooltip("Contains {value, time, duration, setBackValue} in their respective places.")] 
    public float[] defaultSet = new float[] { 1f, 0f, 0f, 0f };

    private void Awake()
    {
        conditionalStat.Add("explosive", false);

        incrementalStat.Add("moveSpeed", defaultSet.ToArray());
        incrementalStat.Add("damage", defaultSet.ToArray());
        incrementalStat.Add("rate", defaultSet.ToArray());
        incrementalStat.Add("attackSpeed", defaultSet.ToArray());
        incrementalStat.Add("shootSpeed", defaultSet.ToArray());
        incrementalStat.Add("capacitor1", defaultSet.ToArray());
        incrementalStat.Add("capacitor2", defaultSet.ToArray());
        incrementalStat.Add("pierces", new float[] { 0f, 0f, 0f, 0f });
        incrementalStat.Add("shields", defaultSet.ToArray());
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
            if (incrementalStat[stat.Key][2] > duration)
            {
                continue;
            }
            incrementalStat[stat.Key][0] += value;
            incrementalStat[stat.Key][3] += value;
            incrementalStat[stat.Key][2] = duration;
            modifiedIncrementals.Add(stat.Key);
        }
    }

    public void ModifyAllDecrementalStats(float value, float duration)
    {
        foreach (var stat in decrementalStat)
        {
            if (decrementalStat[stat.Key][2] > duration)
            {
                continue;
            }
            decrementalStat[stat.Key][0] += value;
            decrementalStat[stat.Key][3] += value;
            decrementalStat[stat.Key][2] = duration;
            modifiedDecrementals.Add(stat.Key);
        }
    }

    public void ModifyIncrementalStat(string name, float value, float duration)
    {
        if (incrementalStat[name][2] > duration)
        {
            return;
        }
        incrementalStat[name][0] += value;
        incrementalStat[name][2] = duration;
        incrementalStat[name][3] += value;
        modifiedIncrementals.Add(name);
    }

    public void ModifyIncrementalStat(string name, float value, bool updateSetBack)
    {
        incrementalStat[name][0] += value;
        if (updateSetBack)
        {
            incrementalStat[name][3] += value;
        }
    }

    public void ModifyDecrementalStat(string name, float value, float duration)
    {
        if (decrementalStat[name][2] > duration)
        {
            return;
        }
        decrementalStat[name][0] += value;
        decrementalStat[name][2] = duration;
        decrementalStat[name][3] += value;
        modifiedDecrementals.Add(name);
    }
    public void ModifyDecrementalStat(string name, float value, bool updateSetBack)
    {
        decrementalStat[name][0] += value;
        if (updateSetBack)
        {
            decrementalStat[name][3] += value;
        }
    }

    public void RevertIncrementalStats()
    {
        foreach (var name in modifiedIncrementals)
        {
            incrementalStat[name][0] -= incrementalStat[name][3];
            incrementalStat[name][1] = 0f;
            incrementalStat[name][2] = 0f;
            incrementalStat[name][3] = 0f;
        }
        modifiedIncrementals.Clear();
    }

    public void RevertDecrementalStats()
    {
        foreach (var name in modifiedDecrementals)
        {
            decrementalStat[name][0] -= decrementalStat[name][3];
            decrementalStat[name][1] = 0f;
            decrementalStat[name][2] = 0f;
            decrementalStat[name][3] = 0f;
        }
        modifiedDecrementals.Clear();
    }

    public void RevertIncrementalStat(string name)
    {
        incrementalStat[name][0] -= incrementalStat[name][3];
        incrementalStat[name][1] = 0f;
        incrementalStat[name][2] = 0f;
        incrementalStat[name][3] = 0f;
        Debug.Log(incrementalStat[name][0] + " / "+ incrementalStat[name][1] + " / " + incrementalStat[name][2]);
        if (modifiedIncrementals.Contains(name))
            modifiedIncrementals.Remove(name);
    }

    public void RevertDecrementalStat(string name)
    {
        decrementalStat[name][0] -= decrementalStat[name][3];
        decrementalStat[name][1] = 0f;
        decrementalStat[name][2] = 0f;
        decrementalStat[name][3] = 0f;
        if (modifiedDecrementals.Contains(name))
            modifiedDecrementals.Remove(name);
    }
}
