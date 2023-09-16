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

    private void Start()
    {
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

    private void OnDestroy()
    {
        OnDeath.RemoveAllListeners();
    }

    public void TakeDamage(float intake, GameObject sender, float strength, int _)
    {
        if (immune)
            return;
        t = strength;
        stats.numericals["health"] -= intake / (shields + 1);
        stats.numericals["shields"] = -1;
        stats.numericals["shields"] = Mathf.Clamp(stats.numericals["shields"], 0, 999999);
        shields = Mathf.RoundToInt(stats.numericals["shields"]);
        healthBar?.Activate();
        if (stats.numericals["health"] <= 0)
        {
            Detach();
            OnDeath.Invoke(sender.transform);
            DestroyStuff();
            if (stats.conditionals["explosive"]){
                PublicPools.pools[stats.explosionPrefab.name].UseObject(transform.position,Quaternion.identity);
            }
        }
    }

    public void TakeInjector(Injector injector)
    {
        if (stats.numericals["health"] <= 0)
            return;
        if (injector.type == injectorType.PLAYER)
            return;
        foreach (string bacteriaPool in injector.bacteriaPools)
        {
            PublicPools.pools[bacteriaPool].SendObject(gameObject);
        }
    }
}
