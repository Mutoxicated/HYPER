using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[DisallowMultipleComponent]
public class Stats : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float VFXScale = 1f;
    public DeathFor objective = DeathFor.PLAYER;
    [HideInInspector] public Transform entity;

    public float maxHealth;
    [SerializeField] private float maxShields;
    [SerializeField] private float shieldhealth = 50f;
    
    [SerializedDictionary("name","boolean")]
    public SerializedDictionary<string, bool> conditionals = new SerializedDictionary<string, bool>(){
        {"explosive", false},
        {"surfaceFXED", false},
        {"outlineFXED", false},
        {"colorFXED", false}
    };
    [SerializedDictionary("stat name","num")]
    public Dictionary<string, float> numericals = new Dictionary<string, float>(){
        {"permaShields",0},
        {"moveSpeed", 1f},
        {"damage", 1f},
        {"hostility",1f},
        {"rate", 1f},
        {"attackSpeed", 1f},
        {"shootSpeed", 1f},
        {"capacitor1", 1f},
        {"capacitor2", 1f},
        {"pierces", 1f},
        {"explosionChance",25f},
        {"extra",0f}
    };

    [HideInInspector] public List<Shield> shields = new List<Shield>();

    private void Awake()
    {
        numericals.Add("health", maxHealth);

        for (int i = 0; i < maxShields;i++){
            shields.Add(new Shield(shieldhealth,false));
        }
    }

    private void OnEnable(){
        numericals["shields"] = maxShields;
        numericals["health"] = maxHealth;
    }

    public void FindEntity(){
        if (objective == DeathFor.PLAYER){
            entity = Difficulty.player;
        }else{
            entity = Difficulty.FindClosestEnemy(transform).transform;
        }
    }

    public void DecideObjective(){
        if (Difficulty.enemies.Count == 1 && Difficulty.enemies[0] == gameObject){
            objective = DeathFor.PLAYER;
            return;
        }
        if (objective == DeathFor.PLAYER){
            objective = DeathFor.ENEMIES;
        }
    }

    public void ChangeObjective(int num){
        objective = (DeathFor)num;
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
