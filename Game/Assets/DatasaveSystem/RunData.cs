using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class EquipmentInfo{
    public string name;
    public int population;

    public EquipmentInfo(string name, int population){
        this.name = name;
        this.population = population;
    }
}

public class RunData 
{
    //run
    public int rounds = 0;
    public string[] goodChosenBacteria;
    public string[] badChosenBacteria;

    //player related
    public int money;    
    public GunShooter.Echelon echelonType = GunShooter.Echelon.IMPOSSIBLE;
    public List<Shield> shields = null;
    public Stats.conditionalDict conditionals = new Stats.conditionalDict();
    public Stats.numericalDict numericals = new Stats.numericalDict();
    public List<EquipmentInfo> equipment = null;
    public int maxEquipment;
    public List<string> activeClassItems = null;
    public List<SuperPassive> activeSuperPassives = null;
    
    //platform generation related
    public int[] generationCycle = null;
    public int cycleIndex = -1;
    public List<PlatformInfo> oldPlatforms = null;
    public List<PlatformInfo> unusedPlatforms = null;

    public RunData(){
        
    }
}
