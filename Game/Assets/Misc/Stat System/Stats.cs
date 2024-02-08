using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine.SceneManagement;

public enum DeathFor {
    PLAYER,
    ENEMIES,
    PLATFORMS,
    PLAYER_FOREVER,
    ENEMIES_FOREVER,
}

[DisallowMultipleComponent]
public class Stats : MonoBehaviour
{
    public bool usePlayerStats;
    public GameObject explosionPrefab;
    public float VFXScale = 1f;
    public EntityType type;
    [SerializeField] private DeathFor[] priority = new DeathFor[3];
    private DeathFor currentLayer;
    [HideInInspector] public Transform entity;
    [HideInInspector] public Transform entityForever;

    public float maxHealth;
    public float range = 28f;
    [SerializeField] private float maxShields;
    [SerializeField] private float shieldhealth = 50f;
    
    [Serializable] public class conditionalDict : SerializableDictionaryBase<string,bool> {};
    //[NonSerialized]
    private static conditionalDict conditionalsPrototype = new conditionalDict(){
        {"explosive", false},
        {"surfaceFXED", false},
        {"outlineFXED", false},
        {"colorFXED", false},
        {"stunned",false}
    };
    public conditionalDict conditionals = new conditionalDict();

    [Serializable] public class numericalDict : SerializableDictionaryBase<string,float> {};
    //[NonSerialized]
    private static numericalDict numericalsPrototype = new numericalDict(){
        {"permaShields",0},
        {"moveSpeed", 1f},
        {"damage", 1f},
        {"hostility",1f},
        {"rate", 1f},
        {"attackSpeed", 1f},
        {"shootSpeed", 1f},
        {"maxCapacitor1", 5f},
        {"capacitor1", 5f},
        {"pierces", 1f},
        {"explosionChance",25f},
        {"extra",0f},
        {"confusion",0f},
        {"parryChance",1f},
        {"shieldHealthModifier",1f},
        {"damageNO",1f},
        {"damageO",1f},
        {"enemyBlockChance",0f},
        {"bacteriaBlockChance",0f},
        {"range",1f},
        {"health", 999f},
        {"maxHealthModifier", 1f},
        {"size",1f},
        {"focus",1f},
        {"regen",1f},
        {"allyDefense",1f}
    };

    public numericalDict numericals = new numericalDict();

    [HideInInspector] public List<Shield> shields = new List<Shield>();

    [ContextMenu("Import scripted dictionaries")]
    private void ImportScriptedDictionaries(){
        numericals = numericalsPrototype;
        conditionals = conditionalsPrototype;
    }

    private void UsePrototypes(){
        foreach (string key in conditionalsPrototype.Keys){
            if (!conditionals.ContainsKey(key))
                conditionals.Add(key,conditionalsPrototype[key]);
        }
        foreach (string key in numericalsPrototype.Keys){
            if (!numericals.ContainsKey(key))
                numericals.Add(key,numericalsPrototype[key]);
        }
    }

    private void Awake()
    {
        if (gameObject.tag == "Player"){
            conditionalDict conds;
            numericalDict nums;
            
            if (RunDataSave.rData.conditionals.Count == 0){
                
                UsePrototypes();
                for (int i = 0; i < maxShields;i++){
                    shields.Add(new Shield(shieldhealth*numericals["shieldHealthModifier"],false));
                }
                numericals["health"] = maxHealth*numericals["maxHealthModifier"];
            }else{
                conds = RunDataSave.rData.conditionals;
                nums = RunDataSave.rData.numericals;
                foreach (string key in conds.Keys){
                    if (!conditionals.ContainsKey(key))
                        conditionals.Add(key,conds[key]);
                    else
                        conditionals[key] = conds[key];
                }
                foreach (string key in nums.Keys){
                    if (!numericals.ContainsKey(key))
                        numericals.Add(key,nums[key]);
                    else
                        numericals[key] = nums[key];
                }
                PlayerInfo.SetMoneyAbsolute(RunDataSave.rData.money);
                shields = RunDataSave.rData.shields.ToList();
            }   

            PlayerInfo.SetShields(shields);
            PlayerInfo.SetConditionals(conditionals);
            PlayerInfo.SetNumericals(numericals);
            return;
        }
        foreach (string key in conditionalsPrototype.Keys){
            if (!conditionals.ContainsKey(key))
                conditionals.Add(key,conditionalsPrototype[key]);
        }
        foreach (string key in numericalsPrototype.Keys){
            if (!numericals.ContainsKey(key))
                numericals.Add(key,numericalsPrototype[key]);
        }
        for (int i = 0; i < maxShields;i++){
            shields.Add(new Shield(shieldhealth*numericals["shieldHealthModifier"],false));
        }
        numericals["health"] = maxHealth*numericals["maxHealthModifier"];
    }

    private void Update(){
        if (entity == null){
            FindEntity();
        }
    }

    public float GetNum(string name){
        if (usePlayerStats)
            return PlayerInfo.GetGun().stats.numericals[name]*numericals[name];
        else
            return numericals[name];
    }

    public void FindEntity(){
        if (EvaluatePriorityLayer(priority[0])){
            currentLayer = priority[0];
            return;
        }
        if (EvaluatePriorityLayer(priority[1])){
            currentLayer = priority[1];
            return;
        }
        if (EvaluatePriorityLayer(priority[2])){
            currentLayer = priority[2];
            return;
        }
    }
    
    private bool IsEntityNull(){
        if (entity == null)
            return true;
        else return false;
    }

    public DeathFor GetCurrentLayer(){
        return currentLayer;
    }

    public DeathFor[] GetPriority(){
        return priority;
    }

    public void SetPriority(DeathFor[] priority){
        this.priority = priority;
    }

    public void OverrideTopPriority(int index){
        priority[0] = (DeathFor)index;
    }

    private bool EvaluatePriorityLayer(DeathFor objective){
        if (objective == DeathFor.PLAYER){
            if (PlayerInfo.GetPlayer() != null){
                if (Vector3.Distance(PlayerInfo.GetPlayer().transform.position,transform.position) > range*numericals["range"])
                    entity = null;
                else{
                    entity = PlayerInfo.GetPlayer().transform;
                    entityForever = PlayerInfo.GetPlayer().transform;;
                }
            }   
            else entity = null;
            return !IsEntityNull();
        }
        if (objective == DeathFor.ENEMIES){
            GameObject go = Difficulty.FindClosestEnemy(transform,range*numericals["range"]);
            if (go == null){
                //Debug.Log("pass");
                entity = null;
            }else{
                entity = go.transform;
                entityForever = go.transform;
            }
            return !IsEntityNull();
        }
        if (objective == DeathFor.PLAYER_FOREVER){
            if (PlayerInfo.GetPlayer() != null){
                entity = PlayerInfo.GetPlayer().transform;
                entityForever = PlayerInfo.GetPlayer().transform;
            }   
            else entity = null;
            return !IsEntityNull();
        }
        if (objective == DeathFor.ENEMIES_FOREVER){
            GameObject go = Difficulty.FindClosestEnemy(transform,999999f);
            if (go == null){
                //Debug.Log("pass");
                entity = null;
            }else{
                entity = go.transform;
                entityForever = go.transform;
            }
            return !IsEntityNull();
        }
        return !IsEntityNull();
        //soon will add platform objective
    }

    public void EnableConditional(string name){
        conditionals[name] = true;
    }

    public void DisableConditional(string name){
        conditionals[name] = false;
    }

    public void IncrementNum(string name, float num){
        numericals[name] = num;
        if (numericals[name] < 0){
            numericals[name] = 0;
        }
    }

    public void ChangeNum(string name, float num){
        numericals[name] = num;
    }

    public void AddShield(int amount){
        for (int i = 0; i < amount;i++){
            shields.Add(new Shield(shieldhealth*numericals["shieldHealthModifier"],false));
        }
    }

    public void GetHealth(float amount){
        numericals["health"] += amount;
    }

    public conditionalDict GetConditionals(){
        return conditionals;
    }

    public numericalDict GetNumericals(){
        return numericals;
    }
}
