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
    [SerializeField] private Stats stats;
    [SerializeField] private Immunity immuneSystem;
    [Header("Post-Death")]
    [SerializeField] private UnityEvent<Transform> OnDeath = new UnityEvent<Transform>();
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject[] ChildrenToDetach;
    [Space]
    [Header("General")]
    [SerializeField] private HitGradient hitGradient;
    [SerializeField] private float immunityDuration;
    public int shields;
    public int maxHP;
    [SerializeField,Range(0.5f,2f)] private float rate = 0.05f;

    [SerializeField] private HealthBar healthBar;

    [HideInInspector] public float currentHP;
    [HideInInspector] public float t = 0f;
    private bool immune = false;
    private float immunityTime;

    private void Awake()
    {
        hitGradient.enabled = false;
    }

    private void Start()
    {
        stats.ModifyIncrementalStat("shields",shields-1,false);
        currentHP = maxHP;
        immune = true;
    }

    private void Update()
    {
        if (immunityTime < immunityDuration)
        {
            immunityTime += Time.deltaTime;
            hitGradient.enabled = false;
            immune = true;
        }
        else
        {
            hitGradient.enabled = true;
            immune = false;
        }
        t = Mathf.Clamp01(t - rate*Time.deltaTime);
    }

    private void RecycleBacteria()
    {
        if (immuneSystem == null)
            return;
        if (immuneSystem.foreignBacteria.Count == 0)
            return;
        foreach (var bac in immuneSystem.foreignBacteria.ToList())
        {
            bac.Instagib();
            immuneSystem.foreignBacteria.Remove(bac);
        }
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
        currentHP -= intake / (shields + 1);
        shields = Mathf.Clamp(shields - 1, 0, 999999);
        stats.incrementalStat["shields"][0] = shields;
        healthBar?.Activate();
        if (currentHP <= 0)
        {
            Debug.Log(currentHP+" ha");
            RecycleBacteria();
            Detach();
            OnDeath.Invoke(sender.transform);
            DestroyStuff();
        }
    }

    public void TakeInjector(Injector injector)
    {
        if (currentHP <= 0)
            return;
        if (injector.type == injectorType.PLAYER)
            return;
        foreach (string bacteriaPool in injector.bacteriaPools)
        {
            PublicPools.pools[bacteriaPool].SendObject(gameObject);
        }
    }
}
