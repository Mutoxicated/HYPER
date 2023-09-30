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
    [SerializeField] private Immunity immuneSystem;
    [Header("Post-Death")]
    [SerializeField] private UnityEvent<Transform> OnDeath = new UnityEvent<Transform>();
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject[] ChildrenToDetach;
    [Space]
    [Header("General")]
    [SerializeField] private float immunityDuration;
    public int shields;
    public int maxHP;
    [SerializeField,Range(0.5f,2f)] private float rate = 0.05f;

    [SerializeField] private HealthBar healthBar;

    [HideInInspector] public float t = 0f;
    [HideInInspector] public bool immune = false;
    private float immunityTime;

    private void OnEnable(){
        Difficulty.enemies.Add(gameObject);
        stats.numericals["shields"] = shields;
        stats.numericals["health"] = maxHP;
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
        OnDeath.RemoveAllListeners();
        immunityTime = 0f;
        stats.numericals["health"] = maxHP;
        stats.numericals["shields"] = shields;
    }

    public void TakeDamage(float intake, GameObject sender, float strength, int _)
    {
        if (immune)
            return;
        t += strength;
        stats.numericals["health"] -= intake / (stats.numericals["shields"] + 1);
        stats.numericals["shields"] = -1;
        stats.numericals["shields"] = Mathf.Clamp(stats.numericals["shields"], 0, 999999);
        stats.numericals["shields"] = Mathf.RoundToInt(stats.numericals["shields"]);
        healthBar?.Activate();
        if (stats.numericals["health"] <= 0)
        {
            if (stats.conditionals["explosive"]){
                PublicPools.pools[stats.explosionPrefab.name].UseObject(transform.position,Quaternion.identity);
            }
            Detach();
            OnDeath.Invoke(sender.transform);
            DestroyStuff();
        }
    }

    public void TakeInjector(Injector injector)
    {
        Debug.Log("acknowledged.");
        if (stats.numericals["health"] <= 0)
            return;
        Debug.Log("health was fine.");
        if (injector.type == injectorType.PLAYER)
            return;
        Debug.Log("injector was fine.");
        foreach (string bacteriaPool in injector.bacteriaPools)
        {
            PublicPools.pools[bacteriaPool].SendObject(gameObject);
        }
    }
}
