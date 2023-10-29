using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public Stats stats;
    public Immunity immuneSystem;
    [Header("Post-Death")]
    public UnityEvent<Transform> OnDeath = new UnityEvent<Transform>();
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject[] ChildrenToDetach;
    [Space]
    [Header("General")]
    [SerializeField] private float immunityDuration;
    [SerializeField,Range(0.5f,2f)] private float rate = 0.05f;

    [SerializeField] private HealthBar healthBar;

    [HideInInspector] public float t = 0f;
    [HideInInspector] public bool immune = false;
    private float immunityTime;

    private void OnEnable(){
        Difficulty.enemies.Add(gameObject);
        immunityTime = 0f;
        t = 0f;
        immune = true;
    }

    private void Update()
    {
        if (immunityTime < immunityDuration)
        {
            immunityTime += Time.deltaTime;
            immune = true;
        }
        else
        {
            immune = false;
        }
        stats.numericals["health"] = Mathf.Clamp(stats.numericals["health"],0,stats.numericals["maxHealth"]);
        if (stats.numericals["health"] < 0)
            stats.numericals["health"] = 0f;
        t = Mathf.Clamp01(t - rate*Time.deltaTime);
    }

    private void DestroyStuff()
    {
        if (ObjectsToDestroy == null)
            return;
        foreach (var obj in ObjectsToDestroy)
        {
            Destroy(obj);
        }
    }

    private void Detach()
    {
        if (ChildrenToDetach == null)
            return;
        foreach (var obj in ChildrenToDetach)
        {
            obj.transform.parent = null;
        }
    }

    private void OnDisable(){
        Difficulty.enemies.Remove(gameObject);
    }

    private void OnDestroy(){
        OnDeath.RemoveAllListeners();
    }

    public void TakeHealth(float intake, float shield){
        stats.numericals["health"] += intake;
        stats.numericals["shields"] += shield;
    }

    public float TakeDamage(float intake, GameObject sender, ref float shieldOut, float strength, int _)
    {
        if (immune)
            return 0f;
         if (stats.numericals["shields"] == 0){
            shieldOut = 0;
        }else{
            shieldOut = 1;
        }
        stats.numericals["health"] -= intake / (stats.shields.Count+stats.numericals["permaShields"] + 1);
        t += strength;
        if (stats.shields.Count > 0){
            if (stats.shields[stats.shields.Count-1].TakeDamage(intake) <= 0f)
                stats.shields.RemoveAt(stats.shields.Count-1);
        }
        healthBar?.Activate();
        if (stats.numericals["health"] <= 0)
        {
            if (stats.conditionals["explosive"] && Random.Range(0f,101f) <= stats.numericals["explosionChance"]){
                PublicPools.pools[stats.explosionPrefab.name].UseObject(transform.position,Quaternion.identity);
            }
            Detach();
            OnDeath.Invoke(sender.transform);
            DestroyStuff();
        }
        if (stats.numericals["health"] >= 0f)
            return 0f;
        else
            return stats.numericals["health"];
    }
    
    public float TakeDamage(float intake, GameObject sender, float strength, int _)
    {
        if (immune)
            return 0f;
        stats.numericals["health"] -= intake / (stats.shields.Count+stats.numericals["permaShields"]+ 1);
        t += strength;
        if (stats.shields.Count > 0){
            if (stats.shields[stats.shields.Count-1].TakeDamage(intake) <= 0f)
                stats.shields.RemoveAt(stats.shields.Count-1);
        }
        healthBar?.Activate();
        if (stats.numericals["health"] <= 0)
        {
            if (stats.conditionals["explosive"] && Random.Range(0f,101f) <= stats.numericals["explosionChance"]){
                PublicPools.pools[stats.explosionPrefab.name].UseObject(transform.position,Quaternion.identity);
            }
            Detach();
            OnDeath.Invoke(sender.transform);
            DestroyStuff();
        }
        if (stats.numericals["health"] >= 0f)
            return 0f;
        else
            return stats.numericals["health"];
    }

    public void TakeInjector(Injector injector, bool cacheInstances)
    {
        //Debug.Log("acknowledged.");
        if (injector.type == injectorType.PLAYER)
            return;
        //Debug.Log("injector was fine.");
        if (stats.numericals["health"] <= 0)
            return;
        //Debug.Log("health was fine.");
        foreach (var bac in injector.allyBacterias)
        {
            if (Random.Range(0f,100f) > injector.chance)
                continue;
            if (cacheInstances){
                Bacteria instancedBac;
                if (immuneSystem.bacterias.ContainsKey(bac.name.Replace("_ALLY",""))){
                    instancedBac = immuneSystem.bacterias[bac.name.Replace("_ALLY","")];
                    immuneSystem.bacterias[bac.name.Replace("_ALLY","")].BacteriaIn();
                }else{
                    instancedBac= PublicPools.pools[bac.name.Replace("_ALLY","")].SendObject(gameObject).GetComponent<Bacteria>();
                }
                instancedBac.injectorCachedFrom = injector;
                injector.cachedInstances.Add(instancedBac);
            }
            else{
                if (immuneSystem.bacterias.ContainsKey(bac.name.Replace("_ALLY",""))){
                    immuneSystem.bacterias[bac.name.Replace("_ALLY","")].BacteriaIn();
                }else{
                    PublicPools.pools[bac.name.Replace("_ALLY","")].SendObject(gameObject);
                }
            }
            Debug.Log(bac.gameObject.name);
        }
    }

    public void RevertInjector(Injector injector){
        foreach (var bac in injector.cachedInstances.ToArray())
        {
            if (immuneSystem.bacterias.ContainsValue(bac))
                immuneSystem.bacterias[bac.name].Instagib();
        }
    }
}
