using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PopulativeInfo{
    public string name;
    public int population;

    public PopulativeInfo(string name, int population){
        this.name = name;
        this.population = population;
    }
}

public class RunData 
{
    //run
    public int seed = -1;
    public int rounds = 0;
    public int runTimeSeconds = 0;
    public bool moneyBonusGot = false;
    public string[] goodChosenBacteria;
    public string[] badChosenBacteria;
    public bool getPassives = true;
    public List<PopulativeInfo> enemyPassives = new List<PopulativeInfo>(){
        new PopulativeInfo("bla",1)
    };
    public List<PassiveItemInfo> passiveSlotsFilled = new List<PassiveItemInfo>(4){
        null,null,null,null,null
    };

    public List<PICSlot> PICVierPassiveItems = new List<PICSlot>(){
        null,null,null,null
    };
    public List<PICSlot> PICTriPassiveItems = new List<PICSlot>(){
        null,null,null
    };
    public List<PICSlot> PICDuoPassiveItems = new List<PICSlot>(){
        null,null
    };
    
    public List<DisabableItem> currentShopItems = new List<DisabableItem>();
    public List<DisabableItem> currentShopEquips = new List<DisabableItem>();
    
    //player related
    public int money;    
    public GunShooter.Echelon echelonType = GunShooter.Echelon.IMPOSSIBLE;
    public List<Shield> shields = null;
    public Stats.conditionalDict conditionals = new Stats.conditionalDict();
    public Stats.numericalDict numericals = new Stats.numericalDict();
    public List<PopulativeInfo> equipment = null;
    public int maxEquipment;
    public List<string> activeClassItems = null;
    public List<SuperPassive> activeSuperPassives = null;
    public Stats.numericalDict superPassiveIterations = new Stats.numericalDict();
    
    //platform generation related
    public int[] generationCycle = null;
    public int cycleIndex = -1;
    public List<PlatformInfo> oldPlatforms = null;
    public List<PlatformInfo> unusedPlatforms = null;

    public RunData(){
        
    }
}
