using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<string,Bacteria> bacterias = new Dictionary<string,Bacteria>();
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
        bacterias.Add(bacteria.name,bacteria);
    }//a

    public void RecycleBacteria()
    {
        if (bacterias.Count == 0)
            return;
        foreach (var bac in bacterias.Values.ToArray())
        {
            bac.Instagib();
        }
    }

    private void Update()
    {
        if (bacterias.Count == 0)
            return;
        t += Time.deltaTime;
        if (t >= immunityAttackRate)
        {
            t = 0f;
            foreach (string bacKey in bacterias.Keys.ToArray())
            {
                //bacterias[bacKey].DamageGoodBacteria();
                if (bacterias[bacKey].immunitySide == ImmunitySide.ALLY ){
                    died = bacterias[bacKey].Degrade(immunityDamage*0.1f*stats.numericals["hostility"]);
                }
                else if (bacterias[bacKey].character == BacteriaCharacter.POSITIVE){
                    died = bacterias[bacKey].Degrade(immunityDamage*0.4f*stats.numericals["hostility"]);
                } else
                {
                    died = bacterias[bacKey].Degrade(immunityDamage*stats.numericals["hostility"]);
                }
                if (died)
                {
                    bacterias.Remove(bacKey);
                }
            }
        }
    }
}
