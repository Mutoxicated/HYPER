using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplestStatModifier : MonoBehaviour
{
        [SerializeField] private bool playerStats;
        [SerializeField] private Stats stats;
        [SerializeField] private PopulativeItem item;

        [SerializeField] private string[] conditionals;
        [SerializeField] private bool[] conditionalValues;

        [SerializeField] private string[] numericals;
        [SerializeField] private float[] initialNumericalIncrements;
        [SerializeField] private float[] numericalIncrements;

        [HideInInspector] public int populationMod;

        private bool effectApplied = false;

        private void ChangeConditionals()
        {
            if (conditionals.Length == 0)
                return;
            for (int i = 0; i < conditionals.Length; i++)
            {
                stats.conditionals[conditionals[i]] = conditionalValues[i];
            }
        }

        private void Start(){
            item.subs.Add(UpdateNumericals);
        }

        private void UpdateNumericals(int addedPopulation){
            if (playerStats)
                stats = PlayerInfo.GetGun().stats;
            if (addedPopulation == 0)
                return;
            if (numericals.Length == 0)
                return;
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] += numericalIncrements[i]*(addedPopulation-1)*EquipmentManager.effectivenessMod;
            }
            populationMod += addedPopulation;
        }

        private void OnEnable()
        {
            if (playerStats)
                stats = PlayerInfo.GetGun().stats;
            populationMod = item.GetPopulation();
            ChangeConditionals();
            if (numericals.Length == 0)
                return;
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] += initialNumericalIncrements[i]*EquipmentManager.effectivenessMod;
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] += numericalIncrements[i]*(populationMod-1)*EquipmentManager.effectivenessMod;
            }
            effectApplied = true;
            Debug.Log("effect applied!");
        }

        private void OnDisable()
        {
            if (!effectApplied)
                return;
            if (stats == null)
                return;
            for (int i = 0; i < conditionals.Length; i++)
            {
                stats.conditionals[conditionals[i]] = !conditionalValues[i];
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] -= initialNumericalIncrements[i]*EquipmentManager.effectivenessMod;
            }
            for (int i = 0; i < numericals.Length; i++)
            {
                stats.numericals[numericals[i]] -= numericalIncrements[i]*(populationMod-1)*EquipmentManager.effectivenessMod;
            }
        }
}
