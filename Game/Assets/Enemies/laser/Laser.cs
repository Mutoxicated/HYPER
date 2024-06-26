using System.Collections.Generic;
using UnityEngine;
using static Numerical;
using static Conditional;

public class Laser : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private bool useObjective;
    public int damage;
    public Injector injector;
    [SerializeField] private OnInterval interval;
    public float maxWidth;
    [SerializeField] private GameObject objToIgnore;
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool dieOnDisable = true;

    private RaycastHit[] hits = new RaycastHit[5];
    private float distance = 100f;
    private float pierces;
    private List<IDamageable> cachedDamageables = new List<IDamageable>();
    private List<float> cachedLeftoverDamage = new List<float>();
    private Vector3 scale;
    private LayerMask mask = -1;
    private List<int> cachedshieldOuts = new List<int>();
    private int refShield;

    public void ParryLaser(GameObject parrier, Vector3 position){
        Debug.Log("parried");
        IDamageable dmgbl = parrier.GetComponent<IDamageable>();
        if (cachedDamageables.Contains(dmgbl)){
            dmgbl.TakeHealth(damage+cachedLeftoverDamage[cachedDamageables.IndexOf(dmgbl)], cachedshieldOuts[cachedDamageables.IndexOf(dmgbl)]);
            dmgbl.RevertInjector(injector);
        }
        GameObject instance = Instantiate(prefab,position,Quaternion.identity);
        var laser = instance.GetComponent<Laser>();
        laser.InheritLaser(this);
        instance.transform.rotation = Quaternion.LookRotation(stats.transform.position-position,Vector3.up);
        instance.SetActive(true);
    }

    public void InheritLaser(Laser laser){
        damage = laser.damage;
        maxWidth = laser.maxWidth;
        injector.injectorToInheritFrom = laser.injector;
    }

    private RaycastHit[] SortRaycasts(RaycastHit[] hits, int hitAmount)
    {
        RaycastHit temp;
        RaycastHit[] sortedHits = hits;

        for (int i = 0; i <= hitAmount - 1; i++)
        {
            for (int j = i + 1; j < hitAmount; j++)
            {
                if (sortedHits[i].distance > sortedHits[j].distance)
                {
                    temp = sortedHits[i];
                    sortedHits[i] = sortedHits[j];
                    sortedHits[j] = temp;
                }
            }
        }
        return sortedHits;
    }

    private Quaternion GetRotation(){
       return Quaternion.LookRotation(stats.entities[0].position - transform.position,Vector3.up);
    }

    private void OnEnable()
    {
        ClearCache();
        if (useObjective){
            if (stats.entities[0] == null){
                stats.FindEntity();
            }
            
            //Debug.Log(stats.entity.gameObject.name);
            transform.rotation = GetRotation();
        }
        pierces = stats.numericals[PIERCES];
        int hitAmount = Physics.SphereCastNonAlloc(transform.position, maxWidth*0.5f, transform.TransformDirection(Vector3.forward), hits, 100f, mask, QueryTriggerInteraction.Ignore);
        hits = SortRaycasts(hits, hitAmount);
        for (int i = 0; i < hitAmount; i++)
        {
            if (objToIgnore == hits[i].collider.gameObject)
                continue;
            
            distance = hits[i].distance;
            //Debug.Log(hits[i].collider.gameObject.name + " | " + i);
            cachedDamageables.Add(hits[i].transform.gameObject.GetComponent<IDamageable>());
            if (stats.conditionals[EXPLOSIVE]){
                PublicPools.pools[stats.explosionPrefab.name].UseObject(hits[i].point,Quaternion.identity);
            }
            if (cachedDamageables[Mathf.RoundToInt(Mathf.Abs(stats.numericals[PIERCES]-pierces))] == null)
                cachedLeftoverDamage.Add(0f);
            else
                cachedLeftoverDamage.Add(cachedDamageables[Mathf.RoundToInt(Mathf.Abs(stats.numericals[PIERCES]-pierces))].TakeDamage(damage, stats,ref refShield,1f,0));
            cachedshieldOuts.Add(refShield);
            cachedDamageables[Mathf.RoundToInt(Mathf.Abs(stats.numericals[PIERCES]-pierces))]?.TakeInjector(injector, true);
            //Debug.Log(Mathf.RoundToInt(Mathf.Abs(stats.numericals[PIERCES]-pierces)));
            if (pierces == 0f)
                break;
            pierces -= 1;
        }
        scale = transform.localScale;
        scale.z = distance*0.1225f;
        transform.localScale = scale;
    }

    public void ClearCache(){
        cachedDamageables.Clear();
        cachedLeftoverDamage.Clear();
    }

    private void OnDisable(){
        if (dieOnDisable){
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (interval.isPlaying){
            scale.x = scale.y = Mathf.Lerp(maxWidth,0f,interval.t);
            transform.localScale = scale;
        }
    }
}
