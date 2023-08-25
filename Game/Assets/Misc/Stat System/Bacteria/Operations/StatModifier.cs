using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private OnInterval optionalInterval;
    [SerializeField, Tooltip("Only if you're using interval. Leave it as 1 if not."), Range(1,10)] private int dispersedAmount;

    [SerializeField] private string[] conditionals;
    [SerializeField] private bool[] conditionalValues;

    [SerializeField] private string[] incrementals;
    [SerializeField] private float[] incrementalAdds;

    [SerializeField] private string[] decrementals;
    [SerializeField] private float[] decrementalAdds;

    private int currentAmount;

    private void ChangeConditionals()
    {
        if (conditionals.Length == 0)
            return;
        for (int i = 0; i < incrementals.Length; i++)
        {
            stats.conditionalStat[conditionals[i]] = conditionalValues[i];
        }
    }

    private void ChangeIncrementals()
    {
        if (incrementals.Length == 0)
            return;
        for (int i = 0; i < incrementals.Length; i++)
        {
            stats.ModifyIncrementalStat(incrementals[i], incrementalAdds[i]/ dispersedAmount, true);
        }
    }

    private void ChangeDecrementals()
    {
        if (decrementals.Length == 0)
            return;
        for (int i = 0; i < decrementals.Length; i++)
        {
            stats.ModifyIncrementalStat(decrementals[i], decrementalAdds[i]/ dispersedAmount, true);
        }
    }

    public void ChangeValues()
    {
        if (currentAmount == dispersedAmount)
        {
            optionalInterval.Pause();
        }
        currentAmount++;
        ChangeIncrementals();
        ChangeDecrementals();
    }

    private void OnEnable()
    {
        stats = GetComponentInParent<Stats>();
        ChangeConditionals();
        if (optionalInterval == null)
        {
            ChangeIncrementals();
            ChangeDecrementals();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < incrementals.Length; i++)
        {
            stats.conditionalStat[conditionals[i]] = !conditionalValues[i];
        }
        for (int i = 0; i < incrementals.Length; i++)
        {
            stats.RevertIncrementalStat(incrementals[i]);
        }
        for (int i = 0; i < decrementals.Length; i++)
        {
            stats.RevertDecrementalStat(decrementals[i]);
        }
    }
}
