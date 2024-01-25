using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunData 
{
    //run
    public bool newRun = true;
    public int money;
    public string[] goodChosenBacteria;
    public string[] badChosenBacteria;
    public GunShooter.Echelon echelonType;
    public List<Shield> shields = new List<Shield>();
    public Stats.conditionalDict conditionals = new Stats.conditionalDict();
    public Stats.numericalDict numericals = new Stats.numericalDict();
    public Dictionary<string, int> equipment = new Dictionary<string, int>();//string refers to the equipment name, int to the population, if the population is 0, that means the equipment was not active
    public List<string> activeClassItems = new List<string>();
    public List<SuperPassive> activeSuperPassives;

    public RunData(){
        
    }
}
