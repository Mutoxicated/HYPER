using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BacteriaOperations{
    public class StatModifier : MonoBehaviour
    {
        [SerializeField] private Bacteria bac;

        [SerializeField] private bool changeObjective;
        [SerializeField] private DeathFor objective;

        [SerializeField] private string[] conditionals;
        [SerializeField] private bool[] conditionalValues;

        [SerializeField] private string[] numericals;
        [SerializeField] private float[] numericalIncrements;

        [HideInInspector] public int populationMod;

        private void ChangeConditionals()
        {
            if (conditionals.Length == 0)
                return;
            for (int i = 0; i < conditionals.Length; i++)
            {
                bac.immuneSystem.stats.conditionals[conditionals[i]] = conditionalValues[i];
            }
        }

        private void ChangeNumericals()
        {
            if (numericals.Length == 0)
                return;
            for (int i = 0; i < numericals.Length; i++)
            {
                bac.immuneSystem.stats.numericals[numericals[i]] += numericalIncrements[i] * (bac.population - populationMod);
            }
            populationMod = bac.population;
        }
        private void Awake(){
            bac.subcribers.Add(ChangeNumericals);
        }

        private void OnEnable()
        {
            // if (bac.immuneSystem == null){
            //     GetComponentInParent<Immunity>().stats.objective = objective;
            // }
            if (changeObjective){
                bac.immuneSystem.stats.objective = objective;
            }
            populationMod = bac.population;
            ChangeConditionals();
            if (numericals.Length == 0)
                return;
            for (int i = 0; i < numericals.Length; i++)
            {
                bac.immuneSystem.stats.numericals[numericals[i]] += numericalIncrements[i]*populationMod;
            }
        }

        private void OnDisable()
        {
            if (changeObjective){
                bac.immuneSystem.stats.objective = DeathFor.PLAYER;
            }
            for (int i = 0; i < conditionals.Length; i++)
            {
                bac.immuneSystem.stats.conditionals[conditionals[i]] = !conditionalValues[i];
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                bac.immuneSystem.stats.numericals[numericals[i]] -= numericalIncrements[i]*populationMod;
            }
        }
    }
}