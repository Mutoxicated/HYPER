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

    private void OnEnable()
    {
        GetComponent<Immunity>().NotifySystem(this);// telling the immune system that we are here, and we are going to kill you
    }

    private float lifeSpan = 100f;

    public bool Degrade(float damage)
    {
        lifeSpan -= damage*strength;
        Debug.Log("oof");
        if (lifeSpan <= 0f)
        {
            Debug.Log("Died! Component Ref: " + this);
            Destroy(this);
            return true;
        }
        return false;
    }
}
