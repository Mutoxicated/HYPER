using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InjectPlayer : MonoBehaviour
{
    [SerializeField] private bool random;
    [SerializeField] private string[] bacteria;
    [SerializeField] private float[] population;
    [SerializeField] private float[] populationStep;

    [SerializeField,Range(0f,100f)] private float chance;
    [SerializeField,Range(0f,10f)] private float chanceStep;

    private int[] cachedPopulations = new int[]{
        0,0,0,0,0,0,0,0,0,0,0
    };

    private void AddBacteriaWithPopulation(string bacteria, int population){
        for (int i = 0; i < population; i++){
            PlayerInfo.GetPH().immuneSystem.injector.AddBacteria(bacteria);
        } 
    }

    private void AddRandomBacteria(int population){
        string bacName = Enum.GetNames(typeof(BacteriaType))[UnityEngine.Random.Range(0,Enum.GetNames(typeof(BacteriaType)).Length)];
        if (UnityEngine.Random.Range(0f,101f) <= 50f){
            bacName += "_ALLY";
        }
        AddBacteriaWithPopulation(bacName, population);
    }

    private int Processed(float num){
        return Mathf.FloorToInt(num);
    }

    private void ApplyEffect(){

        for (int i = 0; i < bacteria.Length; i++){
            int populationToAdd = Processed(population[i])-cachedPopulations[i];
            if (populationToAdd == 0) continue;
            if (!random){

                if (UnityEngine.Random.Range(0f,101f) <= chance){
                    cachedPopulations[i] = Processed(population[i]);
                    AddBacteriaWithPopulation(bacteria[i], populationToAdd);
                }
            }else{
                if (UnityEngine.Random.Range(0f,101f) <= chance){
                    cachedPopulations[i] = Processed(population[i]);
                    AddRandomBacteria(populationToAdd);
                }
            }
        }
    }

    private void Develop(){
        if (chance < 100f)
            chance += chanceStep;
        for (int i = 0; i < population.Length; i++){
            population[i] += populationStep[i];
        }
        ApplyEffect();
    }
}
