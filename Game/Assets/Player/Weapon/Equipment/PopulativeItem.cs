using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulativeItem : MonoBehaviour
{
    public delegate void Sub(int amount);
    public List<Sub> subs = new List<Sub>();
    private int population = 1;

    public int GetPopulation(){
        return population;
    }

    public void AddPopulation(int num){
        population += num;
        foreach (Sub sub in subs){
            sub.Invoke(num);
        }
    }

    private void OnDisable(){
        population = 1;
    }
}
