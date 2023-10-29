using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStatModifier : MonoBehaviour
    {
        [SerializeField] private PassiveItem item;

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
                item.origin.stats.conditionals[conditionals[i]] = conditionalValues[i];
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
                item.origin.stats.numericals[numericals[i]] += initialNumericalIncrements[i];
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                item.origin.stats.numericals[numericals[i]] += numericalIncrements[i]*populationMod;
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
                item.origin.stats.numericals[numericals[i]] -= initialNumericalIncrements[i];
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                item.origin.stats.numericals[numericals[i]] -= numericalIncrements[i]*populationMod;
            }
        }
    }
