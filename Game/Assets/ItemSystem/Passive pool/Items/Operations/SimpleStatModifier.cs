using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Numerical;

public class SimpleStatModifier : MonoBehaviour
{
    [SerializeField] private PassiveItem item;

    [SerializeField] private Conditional[] conditionals;
    [SerializeField] private bool[] conditionalValues;

    [SerializeField] private Numerical[] numericals;
    [SerializeField] private float[] initialNumericalIncrements;
    [SerializeField] private float[] numericalIncrements;

    [HideInInspector] public int populationMod;

    private Dictionary<Numerical, float> limitations = new Dictionary<Numerical, float>(){
        {BACTERIA_BLOCK_CHANCE,0.5f},
        {ENEMY_BLOCK_CHANCE,0.5f},
    };

    private void ChangeConditionals()
    {
        if (conditionals.Length == 0)
            return;
        for (int i = 0; i < conditionals.Length; i++)
        {
            item.origin.stats.conditionals[conditionals[i]] = conditionalValues[i];
        }
    }

    private void ApplyLimitations(){
        for (int i = 0; i < numericals.Length; i++)
        {
            if (limitations.ContainsKey(numericals[i])){
                if (item.origin.stats.numericals[numericals[i]] > limitations[numericals[i]]){
                    item.origin.stats.numericals[numericals[i]] = limitations[numericals[i]];
                }
            }
        }
    }

    private void OnEnable()
    {
        populationMod = item.population;
        ChangeConditionals();
        if (numericals.Length == 0)
            return;
        for (int i = 0; i < numericals.Length; i++)
        {
            item.origin.stats.numericals[numericals[i]] += initialNumericalIncrements[i]*PassivePool.effectivenessMod;
        }
        for (int i = 0; i < numericals.Length; i++)
        {
            item.origin.stats.numericals[numericals[i]] += numericalIncrements[i]*(populationMod-1)*PassivePool.effectivenessMod;;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < conditionals.Length; i++)
        {
            item.origin.stats.conditionals[conditionals[i]] = !conditionalValues[i];
        }
        for (int i = 0; i < numericals.Length; i++)
        {
            item.origin.stats.numericals[numericals[i]] -= initialNumericalIncrements[i]*PassivePool.effectivenessMod;;
        }
        for (int i = 0; i < numericals.Length; i++)
        {
            item.origin.stats.numericals[numericals[i]] -= numericalIncrements[i]*(populationMod-1)*PassivePool.effectivenessMod;;
        }
    }
}
