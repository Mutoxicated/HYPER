using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private Stats stats;
    [Header("Post-Death")]
    [SerializeField] private UnityEvent<Transform> OnDeath = new UnityEvent<Transform>();
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject[] ChildrenToDetach;
    [Space]
    [Header("General")]
    public int maxHP;
    [SerializeField,Range(0.5f,2f)] private float rate = 0.05f;

    [SerializeField] private HealthBar healthBar;

    [HideInInspector] public float currentHP;
    [HideInInspector] public float t = 0f;

    private void Start()
    {
        currentHP = maxHP;
    }

    private void Update()
    {
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

    public void TakeDamage(float intake, GameObject sender)
    {
        t = 1f;
        currentHP -= intake * stats.decrementalStat["defense"];
        healthBar?.Activate();
        if (currentHP <= 0)
        {
            Detach();
            OnDeath.Invoke(sender.transform);
            DestroyStuff();
        }
    }

    public void TakeDamage(float intake)
    {

    }
}
