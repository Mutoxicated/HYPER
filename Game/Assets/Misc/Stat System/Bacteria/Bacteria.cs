using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BacteriaType
{
    FLASH,
    WARDED,
    ERUPTIVE,
    IMPALEMENT,
    MERRY,
    
    BURNING,
    FREEZING,
    POISON,
    RAIN,
    BLINDNESS
}

public class Bacteria : MonoBehaviour
{
    public BacteriaType type;
    [Range(0.9f,0.1f)] public float strength;//weakest to strongest
    private float lifeSpan = 100f;

    private void OnEnable()
    {
        var immuneSystem = GetComponentInParent<Immunity>();
        if (immuneSystem == null)
        {
            Instagib();
            return;
        }
        immuneSystem.NotifySystem(this);// telling the immune system that we are here, and we are going to kill you
    }

    public bool Degrade(float damage)
    {
        lifeSpan -= damage*strength;
        if (lifeSpan <= 0f)
        {
            lifeSpan = 100f;
            PublicPools.pools[gameObject.name].ReattachImmediate(gameObject);
            return true;
        }
        return false;
    }

    public void Instagib()
    {
        lifeSpan = 100f;
        PublicPools.pools[gameObject.name].ReattachImmediate(gameObject);
    }
}
