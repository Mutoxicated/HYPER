using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InjectPlayer : MonoBehaviour
{
    [SerializeField] private bool random;
    [SerializeField] private string[] bacteria;
    [SerializeField] private int[] population;
    [SerializeField] private int[] populationStep;

    [SerializeField,Range(0f,100f)] private float chance;
    [SerializeField,Range(0f,10f)] private float chanceStep;

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

    private void ApplyEffect(){
        for (int i = 0; i < bacteria.Length; i++){
            if (!random){
                if (UnityEngine.Random.Range(0f,101f) <= chance)
                    AddBacteriaWithPopulation(bacteria[i], population[i]);
            }else{
                if (UnityEngine.Random.Range(0f,101f) <= chance)
                    AddRandomBacteria(population[i]);
            }
        }
    }

    private void ApplyEffect(Scene scene, LoadSceneMode lsm){
        if (scene.name != "ArenaV2") return;
        
        for (int i = 0; i < bacteria.Length; i++){
            if (!random){
                if (UnityEngine.Random.Range(0f,101f) <= chance)
                    AddBacteriaWithPopulation(bacteria[i], population[i]);
            }else{
                if (UnityEngine.Random.Range(0f,101f) <= chance)
                    AddRandomBacteria(population[i]);
            }
        }
    }

    private void Start(){
        ApplyEffect();
        SceneManager.sceneLoaded += ApplyEffect;
    }

    private void Develop(){
        if (chance < 100f)
            chance += chanceStep;
        for (int i = 0; i < population.Length; i++){
            population[i] += populationStep[i];
        }
    }
}
