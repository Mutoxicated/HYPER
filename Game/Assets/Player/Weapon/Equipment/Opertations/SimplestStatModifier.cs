using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplestStatModifier : MonoBehaviour
{
        [SerializeField] private Stats stats;
        [SerializeField] private PopulativeItem item;

        [SerializeField] private string[] conditionals;
        [SerializeField] private bool[] conditionalValues;

        [SerializeField] private string[] numericals;
        [SerializeField] private float[] initialNumericalIncrements;
        [SerializeField] private float[] numericalIncrements;

        [HideInInspector] public int populationMod;

        private void ChangeConditionals()
        {
            if (conditionals.Length == 0)
                return;
            for (int i = 0; i < conditionals.Length; i++)
            {
                stats.conditionals[conditionals[i]] = conditionalValues[i];
            }
        }

        private void OnEnable()
        {
            populationMod = item.GetPopulation();
            ChangeConditionals();
            if (numericals.Length == 0)
                return;
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] += initialNumericalIncrements[i];
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] += numericalIncrements[i]*(populationMod-1);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < conditionals.Length; i++)
            {
                stats.conditionals[conditionals[i]] = !conditionalValues[i];
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] -= initialNumericalIncrements[i];
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] -= numericalIncrements[i]*(populationMod-1);
            }
        }
}
