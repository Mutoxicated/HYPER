using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulativeItem : MonoBehaviour
{
    private int population = 1;

    public int GetPopulation(){
        return population;
    }

    public void AddPopulation(int num){
        population += num;
    }

    private void OnDisable(){
        population = 1;
    }
}
