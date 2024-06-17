using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using static Numerical;
using static Conditional;

public enum DeathFor {
    PLAYER,
    ENEMIES,
    PLATFORMS,
    PLAYER_FOREVER,
    ENEMIES_FOREVER,
}

public enum Numerical {
    HEALTH,
    SHIELD_HEALTH,
    MOVE_SPEED,
    RATE,
    HOSTILITY,
    ATTACK_SPEED,
    MAX_CAPACITOR_1,
    CAPACITOR_1,
    PIERCES,
    EXPLOSION_CHANCE,
    CONFUSION,
    PARRY_CHANCE,
    SHIELD_HEALTH_MODIFIER,
    DAMAGE_NO,
    DAMAGE_O,
    ENEMY_BLOCK_CHANCE,
    BACTERIA_BLOCK_CHANCE,
    RANGE,
    MAX_HEALTH_MODIFIER,
    SIZE,
    FOCUS,
    REGEN,
    ALLY_DEFENSE,
    PERMA_SHIELDS
}

public enum Conditional {
    EXPLOSIVE,
    SURFACE_FXED,
    OUTLINE_FXED,
    COLOR_FXED,
    STUNNED
}

[DisallowMultipleComponent]
public class Stats : MonoBehaviour, IStatAffectable
{
    [Header("Extra")]
    public bool usePlayerStats;
    public GameObject explosionPrefab;
    public float VFXScale = 1f;

    [Header("Settings")]
    public EntityType type;
    [SerializeField] private DeathFor[] priority = new DeathFor[3];
    private DeathFor currentLayer;

    public float maxHealth;
    public float range = 28f;
    [SerializeField] private float maxShields;
    [SerializeField] private float shieldhealth = 50f;
    
    [Serializable] public class conditionalDict : SerializableDictionaryBase<Conditional,bool> {};
    //[NonSerialized]
    [Header("Stats")]
    private static conditionalDict conditionalsPrototype = new conditionalDict(){
        {EXPLOSIVE, false},
        {SURFACE_FXED, false},
        {OUTLINE_FXED, false},
        {COLOR_FXED, false},
        {STUNNED, false}
    };
    public conditionalDict conditionals = new conditionalDict();

    [Serializable] public class numericalDict : SerializableDictionaryBase<Numerical,float> {};
    //[NonSerialized]
    private static numericalDict numericalsPrototype = new numericalDict(){
        {PERMA_SHIELDS, 0f},
        {MOVE_SPEED, 1f},
        {HOSTILITY, 1f},
        {RATE, 1f},
        {ATTACK_SPEED, 1f},
        {MAX_CAPACITOR_1, 5f},
        {CAPACITOR_1, 5f},
        {PIERCES, 1f},
        {EXPLOSION_CHANCE, 25f},
        {CONFUSION, 0f},
        {PARRY_CHANCE, 1f},
        {SHIELD_HEALTH_MODIFIER, 1f},
        {DAMAGE_NO, 1f},
        {DAMAGE_O, 1f},
        {ENEMY_BLOCK_CHANCE, 0f},
        {BACTERIA_BLOCK_CHANCE, 0f},
        {RANGE, 1f},
        {HEALTH, 999f},
        {MAX_HEALTH_MODIFIER, 1f},
        {SIZE, 1f},
        {FOCUS, 1f},
        {REGEN, 1f},
        {ALLY_DEFENSE, 1f},
    };

    public numericalDict numericals = new numericalDict();

    [HideInInspector] public List<Shield> shields = new List<Shield>();
    private bool initiatedDictionaries = false;

    [HideInInspector] public List<Transform> entities = new List<Transform>() {
        null
    };
    static List<Transform> Empty = new List<Transform>() {
        null
    };

    [ContextMenu("Import scripted dictionaries")]
    private void ImportScriptedDictionaries(){
        numericals.Clear();
        numericals = numericalsPrototype;
        conditionals = conditionalsPrototype;
        initiatedDictionaries = true;
    }

    private void UsePrototypes(){
        foreach (var key in conditionalsPrototype.Keys){
            if (!conditionals.ContainsKey(key))
                conditionals.Add(key,conditionalsPrototype[key]);
        }
        foreach (var key in numericalsPrototype.Keys){
            if (!numericals.ContainsKey(key))
                numericals.Add(key,numericalsPrototype[key]);
        }
        initiatedDictionaries = true;
    }

    private void Awake()
    {
        if (gameObject.tag == "Player"){
            conditionalDict conds;
            numericalDict nums;
            
            if (RunDataSave.rData.conditionals.Count == 0){
                
                UsePrototypes();
                for (int i = 0; i < maxShields;i++){
                    shields.Add(new Shield(shieldhealth*numericals[SHIELD_HEALTH_MODIFIER],false));
                }
                numericals[HEALTH] = maxHealth*numericals[MAX_HEALTH_MODIFIER];
            }else{
                conds = RunDataSave.rData.conditionals;
                nums = RunDataSave.rData.numericals;
                foreach (var key in conds.Keys){
                    if (!conditionals.ContainsKey(key))
                        conditionals.Add(key,conds[key]);
                    else
                        conditionals[key] = conds[key];
                }
                foreach (var key in nums.Keys){
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
        UsePrototypes();
        for (int i = 0; i < maxShields;i++){
            shields.Add(new Shield(shieldhealth*numericals[SHIELD_HEALTH_MODIFIER],false));
        }
        numericals[HEALTH] = maxHealth*numericals[MAX_HEALTH_MODIFIER];
    }

    private void Update(){
        if (entities[0] == null){
            FindEntity();
        }
    }

    public float GetNum(Numerical name){
        if (usePlayerStats)
            return PlayerInfo.GetGun().stats.numericals[name]*numericals[name];
        else
            return numericals[name];
    }

    public void FindEntity(){
        if (!initiatedDictionaries) {
            UsePrototypes();
        }
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
        if (entities[0] == null)
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

    public void SetPriorityLayer(int index, DeathFor df){
        priority[index] = df;
    }

    public DeathFor GetPriorityLayer(int index){
        return priority[index];
    }

    public void OverrideTopPriority(int index){
        priority[0] = (DeathFor)index;
    }

    private void UpdateEntities(GameObject[] gos) {
        entities.Clear();
        foreach (var go in gos) {
            entities.Add(go.transform);
        }
        if (entities.Count == 0) {
            ResetEntities();
        }
    }

    private void ResetEntities() {
        entities = Empty;
    }

    private void SetTag(string tag) {
        if (gameObject.CompareTag("Player")) {
            return;
        }
        if (!gameObject.CompareTag(tag)) {
            gameObject.tag = tag;
        }
    }

    private bool EvaluatePriorityLayer(DeathFor objective){
        if (objective == DeathFor.PLAYER){
            Debug.Log(gameObject.name+" got PLAYER");
            SetTag("Enemy");
            if (PlayerInfo.GetPlayer() != null){
                if (Vector3.Distance(PlayerInfo.GetPlayer().transform.position,transform.position) > range*numericals[RANGE])
                    ResetEntities();
                else{
                    entities[0] = PlayerInfo.GetPlayer().transform;
                }
            }   
            else ResetEntities();
            return !IsEntityNull();
        }
        if (objective == DeathFor.ENEMIES){
            Debug.Log(gameObject.name+" got ENEMIES");
            SetTag("Untagged");
            GameObject[] gos = Difficulty.FindClosestEntities(transform,range*numericals[RANGE]);
            if (gos.Length == 0){
                //Debug.Log("pass");
                ResetEntities();
            }else{
                UpdateEntities(gos);
            }
            return !IsEntityNull();
        }
        if (objective == DeathFor.PLAYER_FOREVER){
            Debug.Log(gameObject.name+" got PLAYER_FOREVER");
            SetTag("Enemy");
            if (PlayerInfo.GetPlayer() != null){
                entities[0] = PlayerInfo.GetPlayer().transform;
            }   
            else ResetEntities();
            return !IsEntityNull();
        }
        if (objective == DeathFor.ENEMIES_FOREVER){
            Debug.Log(gameObject.name+" got ENEMIES_FOREVER");
            SetTag("Untagged");
            GameObject[] gos = Difficulty.FindClosestEntities(transform,range*numericals[RANGE]);
            if (gos.Length == 0){
                //Debug.Log("pass");
                ResetEntities();
            }else{
                UpdateEntities(gos);
            }
            return !IsEntityNull();
        }
        return !IsEntityNull();
        //soon will add platform objective
    }

    public void AddShield(int amount){
        for (int i = 0; i < amount;i++){
            shields.Add(new Shield(shieldhealth*numericals[SHIELD_HEALTH_MODIFIER],false));
        }
    }

    public conditionalDict GetConditionals(){
        return conditionals;
    }

    public numericalDict GetNumericals(){
        return numericals;
    }

    public void ModifyNumerical(Numerical name, float value, Modification modType) {
        if (modType == Modification.RELATIVE) {
            numericals[name] += value;
            return;
        }
        numericals[name] = value;
    }

    public void ModifyConditional(Conditional name, bool state) {
        conditionals[name] = state;
    }
}
