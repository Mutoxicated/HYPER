using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BacteriaType
{
    FLASH,
    WARDED,
    ERUPTION,
    IMPALEMENT,
    MERRY,

    BURNING,
    FREEZING,
    POISON,
    RAIN,
    CHIMERA,
    SLOTH,

    RADIATION,
    FLABBERGAST,
    BRAG,
    TASTY,
    CORRUPT,
    VAMPIRE,
    BETRAY,
}

public enum ImmunitySide
{
    ALLY,
    INVADER
}

public enum BacteriaCharacter // This means: Generally does it negatively impact entities or positively?
{
    NEGATIVE,
    POSITIVE
}

public class Bacteria : MonoBehaviour
{
    public ImmunitySide immunitySide;
    public BacteriaInfo ID;
    [HideInInspector] public Injector injectorCachedFrom;
    [SerializeField] private ParticleSystem _particleSys;
    [SerializeField] private OnInterval interval;
    [Range(1f, 0.1f)] public float strength;//weakest to strongest
    [SerializeField] private float damage;
    public float lifeSpan = 100f;
    [HideInInspector] public Immunity immuneSystem;
    public int population = 1;
    public delegate void Subcriber();
    public List<Subcriber> subcribers = new List<Subcriber>();

    private EffectManager fxm;
    private float initEmissionRate;

    private void Awake(){
        fxm = GetComponent<EffectManager>();
        if (_particleSys == null)
            return;
        var emission = _particleSys.emission;
        initEmissionRate = emission.rateOverTime.constant;
    }

    private void OnEnable()
    {
        ChangeEmission();
        immuneSystem = transform.parent.GetComponent<Immunity>();
        transform.localScale = Vector3.one * immuneSystem.stats.VFXScale;
        if (immuneSystem == null)
        {
            RemoveSelfFromInjector();
            Instagib();
            return;
        }
        immuneSystem.NotifySystem(this);// telling the immune system that we are here, and we are going to kill you, or help you!
        if (immunitySide == ImmunitySide.INVADER && interval != null)
        {
            interval.enabled = true;
        }
        CheckInjector();
        if (fxm != null){
            fxm.SetEffect();
        }
    }
    private void ChangeEmission(){
        if (_particleSys == null)
            return;
        var emission = _particleSys.emission;
        emission.rateOverTime = initEmissionRate*population;
    }

    private void InvokeSubs(){
        foreach (Subcriber sub in subcribers){
            sub();
        }
    }

    public void BacteriaIn(){
        population++;
        ChangeEmission();
        InvokeSubs();
    }

    public bool BacteriaOut(){
        if (population <= 1){
            if (interval != null)
            {
                interval.ResetEventless();
                interval.enabled = false;
            }
            RemoveSelfFromInjector();
            lifeSpan = 100f;
            PublicPools.pools[gameObject.name].ReattachImmediate(gameObject);
            return true;
        }else{
            population--;
            lifeSpan = 100f;
            ChangeEmission();
            InvokeSubs();
            return false;
        }
    }

    private void CheckInjector()
    {
        if (immunitySide == ImmunitySide.INVADER)
            return;
        if (!immuneSystem.injector.allyBacterias.Contains(this))
        {
            immuneSystem.injector.allyBacterias.Add(this);
        }

    }

    public void DamageGoodBacteria()
    {
        if (ID.character == BacteriaCharacter.POSITIVE)
            return;
        if (immunitySide == ImmunitySide.ALLY)
            return;
        if (immuneSystem.bacterias.Count == 0)
            return;
        foreach (var bac in immuneSystem.bacterias.Values.ToArray())
        {
            if (bac.ID.character == BacteriaCharacter.POSITIVE)
            {
                bac.Degrade(damage);
            }
            else if (bac.immunitySide == ImmunitySide.ALLY)
            {
                bac.Degrade(damage);
            }
        }
    }

    public bool Degrade(float damage)
    {
        lifeSpan -= damage * strength;
        if (lifeSpan <= 0f)
        {
            return BacteriaOut();
        }
        return false;
    }

    private void RemoveSelfFromInjector()
    {
        RemoveSelfFromCachedInstances();
        if (immunitySide == ImmunitySide.INVADER)
            return;
        immuneSystem.injector.allyBacterias.Remove(this);
    }

    private void RemoveSelfFromCachedInstances(){
        if (injectorCachedFrom != null)
            injectorCachedFrom.cachedInstances.Remove(this);
        injectorCachedFrom = null;
    }

    public void Instagib()
    {
        if (interval != null)
        {
            interval.ResetEventless();
            interval.enabled = false;
        }
        RemoveSelfFromInjector();
        population = 1;
        lifeSpan = 100f;
        PublicPools.pools[gameObject.name].ReattachImmediate(gameObject);
        if (immuneSystem.bacterias.ContainsValue(this)){
            immuneSystem.bacterias.Remove(name);
        }
    }
}
