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

    private void Inject(IDamageable damage){
        if (health == null)
            return;
        if (health.immuneSystem.injector == null)
            return;
        damage?.TakeInjector(health.immuneSystem.injector, false);
    }

    private void Start(){
        if (gameObject.name == "PLAYER_LARGE_EXP"){
            SuperPassivePool.DevelopPassiveByName("NUCLEAR");
        }
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
        float mod = 1f;
        if (gameObject.name == "PLAYER_LARGE_EXP")
            mod = TNT.tntEffectiveness;
        if (damageOutput > 0)
        {
            damageable?.TakeDamage(damageOutput * stats.GetNum("damage")*mod, stats,1f,0);
        }
        Inject(damageable);
        health?.TakeDamage(damageInput,stats,1f,0);
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
        float mod = 1f;
        if (gameObject.name == "PLAYER_LARGE_EXP")
            mod = TNT.tntEffectiveness;
        if (damageOutput > 0)
        {
            damageable?.TakeDamage(damageOutput * stats.GetNum("damage")*mod, stats, 1f, 0);
        }
        Inject(damageable);
        health?.TakeDamage(damageInput,stats,1f,0);
    }
}
