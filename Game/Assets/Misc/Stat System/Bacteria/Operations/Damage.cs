using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Bacteria bac;
    public OnInterval interval;
    public float damage;
    public int hurtScreenIndex;
    private float _;
    private IDamageable cachedDamageable;

    public void DamageEntity()
    {
        cachedDamageable.TakeDamage(damage*bac.population,gameObject, ref _, 0.1f, hurtScreenIndex);
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        cachedDamageable = GetComponentInParent<IDamageable>();
    }
}
