using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public OnInterval interval;
    public float damage;
    public int hurtScreenIndex;

    private IDamageable cachedDamageable;

    public void DamageEntity()
    {
        cachedDamageable.TakeDamage(damage,gameObject, -1f, hurtScreenIndex);
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        cachedDamageable = GetComponentInParent<IDamageable>();
    }
}
