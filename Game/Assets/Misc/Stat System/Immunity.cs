using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    ORGANIC,
    NON_ORGANIC
}

public class Immunity : MonoBehaviour
{
    public Stats stats;
    public Injector injector;
    public EntityType type;
    public float immunityAttackRate = 1f;
    public float immunityDamage = 10f;
    [HideInInspector]
    public List<Bacteria> foreignBacteria = new List<Bacteria>();
    private float t;
    private bool died;

    private void OnEnable(){
        if (stats == null)
            stats = GetComponent<Stats>();
    }

    public void NotifySystem(Bacteria bacteria)
    {
        //Debug.Log("Notified of bacteria "+bacteria.name+ ".");
        if (type == EntityType.ORGANIC)
        {
            if (bacteria.type == BacteriaType.RAIN)
            {
                bacteria.Instagib();
                return;
            }
        }
        else
        {
            if (bacteria.type == BacteriaType.POISON)
            {
                Debug.Log("whataaa?");
                bacteria.Instagib();
                return;
            }
        }
        foreignBacteria.Add(bacteria);
    }

    public void RecycleBacteria()
    {
        if (foreignBacteria.Count == 0)
            return;
        foreach (var bac in foreignBacteria.ToArray())
        {
            bac.Instagib();
            foreignBacteria.Remove(bac);
        }
        injector.allyBacterias.Clear();
    }

    private void Update()
    {
        if (foreignBacteria.Count == 0)
            return;
        t += Time.deltaTime;
        if (t >= immunityAttackRate)
        {
            t = 0f;
            foreach (Bacteria bacteria in foreignBacteria.ToArray())
            {
                bacteria.DamageGoodBacteria();
                if (bacteria.immunitySide == ImmunitySide.ALLY ){
                    died = bacteria.Degrade(immunityDamage*0.2f);
                }
                else if (bacteria.bacChar == BacteriaCharacter.POSITIVE){
                    died = bacteria.Degrade(immunityDamage*0.5f);
                } else
                {
                    died = bacteria.Degrade(immunityDamage);
                }
                if (died)
                {
                    if (bacteria.immunitySide == ImmunitySide.ALLY){
                        injector.bacteriaPools.Remove(bacteria.gameObject.name.Replace("_ALLY",""));
                    }
                    foreignBacteria.Remove(bacteria);
                }
            }
        }
    }
}
