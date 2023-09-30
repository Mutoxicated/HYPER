using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private EnemyHealth health;
    [SerializeField] private float damageOutput;
    [SerializeField] private float damageInput;
    [SerializeField] private LayerMask layersToIgnore;
    [SerializeField] private string[] tagsToIgnore;

    private IDamageable damageable;
    private Injector injector;

    private void OnEnable()
    {
        injector = GetComponent<Injector>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (tagsToIgnore != null)
        {
            foreach (var tag in tagsToIgnore)
            {
                if (collision.gameObject.tag == tag)
                    return;
            }
        }
        if ((layersToIgnore & (1 << collision.collider.gameObject.layer)) != 0)
            return;
        damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageOutput > 0)
        {
            damageable?.TakeDamage(damageOutput * stats.numericals["damage"], gameObject,1f,0);
        }
        if (injector != null)
        {
            if (injector.injectEnabled)
                damageable?.TakeInjector(injector);
        }
        health?.TakeDamage(damageInput,gameObject,1f,0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tagsToIgnore != null)
        {
            foreach (var tag in tagsToIgnore)
            {
                if (other.gameObject.tag == tag)
                    return;
            }
        }
        if ((layersToIgnore & (1 << other.gameObject.layer)) != 0)
            return;
        damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageOutput > 0)
        {
            damageable?.TakeDamage(damageOutput * stats.numericals["damage"], gameObject, 1f, 0);
        }
        if (injector != null)
        {
            if (injector.injectEnabled)
                damageable?.TakeInjector(injector);
        }
        if (stats.conditionals["explosive"])
        {
            PublicPools.pools[stats.explosionPrefab.name].UseObject(transform.position, Quaternion.identity);
        }
        health?.TakeDamage(damageInput,gameObject,1f,0);
    }
}
