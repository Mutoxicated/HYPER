using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier : MonoBehaviour
{
    [SerializeField] private Stats stats;

    [SerializeField] private string[] conditionals;
    [SerializeField] private bool[] conditionalValues;

    [SerializeField] private string[] numericals;
    [SerializeField] private float[] numericalIncrements;

    private void ChangeConditionals()
    {
        if (conditionals.Length == 0)
            return;
        for (int i = 0; i < conditionals.Length; i++)
        {
            stats.conditionals[conditionals[i]] = conditionalValues[i];
        }
    }

    private void ChangeNumericals()
    {
        if (numericals.Length == 0)
            return;
        for (int i = 0; i < numericals.Length; i++)
        {
            stats.numericals[numericals[i]] += numericalIncrements[i];
        }
    }

    private void OnEnable()
    {
        stats = GetComponentInParent<Stats>();
        ChangeConditionals();
        ChangeNumericals();
    }

    private void OnDisable()
    {
        for (int i = 0; i < conditionals.Length; i++)
        {
            stats.conditionals[conditionals[i]] = !conditionalValues[i];
        }
        for (int i = 0; i < numericals.Length; i++)
        {
            stats.numericals[numericals[i]] -= numericalIncrements[i];
        }
    }
}
