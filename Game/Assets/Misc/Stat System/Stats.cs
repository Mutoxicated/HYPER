using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public enum DeathFor {
    PLAYER,
    ENEMIES,
    PLAYER_FOREVER,
    ENEMIES_FOREVER
}

[DisallowMultipleComponent]
public class Stats : MonoBehaviour
{
    [SerializeField] private bool inheritFromPlayer;
    public GameObject explosionPrefab;
    public float VFXScale = 1f;
    public EntityType type;
    public DeathFor objective = DeathFor.PLAYER;
    [HideInInspector] public Transform entity;

    public float maxHealth;
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
        {"capacitor1", 1f},
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
        {"range",28f},
        {"health", 999f},
        {"maxHealthModifier", 1f}
    };

    public numericalDict numericals = new numericalDict();

    [HideInInspector] public List<Shield> shields = new List<Shield>();

    [ContextMenu("Import scripted dictionaries")]
    private void ImportScriptedDictionaries(){
        numericals = numericalsPrototype;
        conditionals = conditionalsPrototype;
    }

    private void Awake()
    {
        foreach (string key in conditionalsPrototype.Keys){
            if (!conditionals.ContainsKey(key))
                conditionals.Add(key,conditionalsPrototype[key]);
        }
        foreach (string key in numericalsPrototype.Keys){
            if (!numericals.ContainsKey(key))
                numericals.Add(key,numericalsPrototype[key]);
        }
        numericals["health"] = maxHealth;
        if (inheritFromPlayer){
            numericals = PlayerInfo.GetGun().stats.numericals;
            conditionals = PlayerInfo.GetGun().stats.conditionals;
        }
    }

    private void Start(){
        for (int i = 0; i < maxShields;i++){
            shields.Add(new Shield(shieldhealth*numericals["shieldHealthModifier"],false));
        }
    }

    public void FindEntity(){
        if (objective == DeathFor.PLAYER || objective == DeathFor.PLAYER_FOREVER){
            entity = PlayerInfo.GetPlayer().transform;
        }else{
            GameObject go = Difficulty.FindClosestEnemy(transform,numericals["range"]);
            //Debug.Log("From go: "+gameObject.name+", found go: "+go.name);
            if (go == null){
                //Debug.Log("pass");
                entity = null;
            }else{
                entity = go.transform;
            }
        }
    }

    public void DecideObjective(){
        if (Difficulty.enemies.Count == 1 && Difficulty.enemies[0] == gameObject && objective == DeathFor.ENEMIES){
            objective = DeathFor.PLAYER;
            return;
        }
        if (objective == DeathFor.PLAYER && Difficulty.player == null){
            objective = DeathFor.ENEMIES;
        }
    }

    public void ChangeObjective(int num){
        objective = (DeathFor)num;
        FindEntity();
    }
    public void ChangeObjectiveToOpposite(){
        if (objective == DeathFor.PLAYER){
            objective = DeathFor.ENEMIES;
        }else{
            objective = DeathFor.PLAYER;
        }
        FindEntity();
    }

    public void ChangeObjective(DeathFor objective){
        this.objective = objective;
        FindEntity();
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
}
