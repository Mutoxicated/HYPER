using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Numerical;

public class OozeOut : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private GameObject oozePrefab;
    
    private List<ParticleCollisionEvent> pces = new List<ParticleCollisionEvent>();

    private void Start(){
        var emission = ps.emission;
        emission.rateOverTimeMultiplier = stats.numericals[RATE];
    }

    private void OnParticleCollision(GameObject other){
        ps.GetCollisionEvents(other,pces);
        PublicPools.pools[oozePrefab.name].UseObject(pces[pces.Count-1].intersection,Quaternion.identity);
    }
}
