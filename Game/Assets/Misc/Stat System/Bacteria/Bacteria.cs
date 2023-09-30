using System.Collections;
using System.Collections.Generic;
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
    BRAG
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
    public BacteriaType type;
    public ImmunitySide immunitySide;
    public BacteriaCharacter bacChar;
    [SerializeField] private OnInterval interval;
    [Range(1f,0.1f)] public float strength;//weakest to strongest
    [SerializeField] private float damage;
    [HideInInspector]
    public float lifeSpan = 100f;
    [HideInInspector]
    public Immunity immuneSystem;

    private void OnEnable()
    {
        immuneSystem = GetComponentInParent<Immunity>();
        transform.localScale = Vector3.one*immuneSystem.stats.VFXScale;
        if (immuneSystem == null)
        {
            RemoveSelfFromInjector();
            Instagib();
            return;
        }
        if (immunitySide == ImmunitySide.INVADER && interval != null)
        {
            interval.enabled = true;
        }
        Invoke("CheckInjector",0.01f);
        immuneSystem.NotifySystem(this);// telling the immune system that we are here, and we are going to kill you, or help you!
    }

    private void CheckInjector(){
        if (immunitySide == ImmunitySide.INVADER)
            return;
        if (!immuneSystem.injector.allyBacterias.Contains(this)){
            immuneSystem.injector.allyBacterias.Add(this);
            if (immuneSystem.injector.bacteriaPools.Contains(gameObject.name.Replace("_ALLY","")))
                return;
            immuneSystem.injector.bacteriaPools.Add(gameObject.name.Replace("_ALLY",""));
        }
        
    }

    public void DamageGoodBacteria()
    {
        if (bacChar == BacteriaCharacter.POSITIVE)
            return;
        if (immunitySide == ImmunitySide.ALLY)
            return;
        if (immuneSystem.foreignBacteria.Count == 0)
            return;
        foreach (var bac in immuneSystem.foreignBacteria)
        {
            if (bac.bacChar == BacteriaCharacter.POSITIVE)
            {
                bac.Degrade(damage);
            }else if (bac.immunitySide == ImmunitySide.ALLY){
                bac.Degrade(damage);
            }
        }
    }

    public bool Degrade(float damage)
    {
        lifeSpan -= damage*strength;
        if (lifeSpan <= 0f)
        {
            if (interval != null)
            {
                interval.ResetEventless();
                interval.enabled = false;
            }
            RemoveSelfFromInjector();
            lifeSpan = 100f;
            PublicPools.pools[gameObject.name].ReattachImmediate(gameObject);
            return true;
        }
        return false;
    }

    private void RemoveSelfFromInjector(){
        if (immunitySide == ImmunitySide.INVADER)
            return;
        immuneSystem.injector.bacteriaPools.Remove(gameObject.name.Replace("_ALLY",""));
    }

    public void Instagib()
    {
        if (interval != null){
            interval.ResetEventless();
            interval.enabled = false;
        }
        lifeSpan = 100f;
        PublicPools.pools[gameObject.name].ReattachImmediate(gameObject);
    }
}
