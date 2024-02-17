using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BacteriaOperations{
    public enum PriorityChangeType{
        OPPOSITE,
        OPPOSITE_FOREVER
    }

    public class StatModifier : MonoBehaviour
    {
        [SerializeField] private Bacteria bac;

        [SerializeField] private bool changePriority;
        [SerializeField] private PriorityChangeType[] priority = new PriorityChangeType[3];

        [SerializeField] private string[] conditionals;
        [SerializeField] private bool[] conditionalValues;

        [SerializeField] private string[] numericals;
        [SerializeField] private float[] numericalIncrements;

        [HideInInspector] public int populationMod;

        private DeathFor[] cachedPriority = new DeathFor[3];

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

        private bool EdgeCasesPresent(){
            if (bac.immuneSystem.injector.injectorToInheritFrom == PlayerInfo.GetPH().immuneSystem.injector && bac.immuneSystem.stats.usePlayerStats)
                return true;
            return false;
        }

        private DeathFor ChangePriority(DeathFor df, PriorityChangeType pct){
            if (df == DeathFor.PLAYER | df == DeathFor.PLAYER_FOREVER){
                if (pct == PriorityChangeType.OPPOSITE_FOREVER){
                    return DeathFor.ENEMIES_FOREVER;
                }else{
                    return DeathFor.ENEMIES;
                }
            }else if (df == DeathFor.ENEMIES | df == DeathFor.ENEMIES_FOREVER){
                if (pct == PriorityChangeType.OPPOSITE_FOREVER){
                    return DeathFor.PLAYER_FOREVER;
                }else{
                    return DeathFor.PLAYER;
                }
            }else if (df == DeathFor.PLATFORMS){
                return DeathFor.ENEMIES;
            }
            return DeathFor.ENEMIES;
        }

        private void OnEnable()
        {
            if (EdgeCasesPresent())
                return;
            // if (bac.immuneSystem == null){
            //     GetComponentInParent<Immunity>().stats.objective = objective;
            // }
            if (changePriority){
                cachedPriority = bac.immuneSystem.stats.GetPriority();
                for (int i = 0; i < priority.Length; i++){
                    bac.immuneSystem.stats.SetPriorityLayer(i, ChangePriority(bac.immuneSystem.stats.GetPriorityLayer(i),priority[i]));
                }
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
            if (EdgeCasesPresent())
                return;
            if (changePriority){
                bac.immuneSystem.stats.SetPriority(cachedPriority);
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