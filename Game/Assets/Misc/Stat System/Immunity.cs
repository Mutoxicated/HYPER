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
    public float immunityAttackRate = 1f;
    public float immunityDamage = 10f;
    [SerializeField] private BacteriaType[] specialImmunities;
    [HideInInspector]
    public Dictionary<string,Bacteria> bacterias = new Dictionary<string,Bacteria>();
    private float t;
    private bool died;

    private void OnEnable(){
        if (stats == null)
            stats = GetComponent<Stats>();
    }

    private void CheckSpecialImmunities(Bacteria bac){
        if (specialImmunities.Length == 0) return;
        foreach (BacteriaType bt in specialImmunities){
            if (bac.type == bt){
                bac.Instagib();
                return;
            }
        }
    }

    private void CheckOrganicImmunities(Bacteria bac){
        if (stats.type == EntityType.NON_ORGANIC)
            return;
        if (bac.immunitySide == ImmunitySide.ALLY)
            return;
        switch (bac.type) {
            case BacteriaType.RAIN:
                bac.Instagib();
                break;
            // case BacteriaType.RADIATION:
            //     bac.Instagib();
            //     break;
            default:
                break;
        }
    }
    private void CheckNonOrganicImmunities(Bacteria bac){
        if (stats.type == EntityType.ORGANIC)
            return;
        if (bac.immunitySide == ImmunitySide.ALLY)
            return;
        switch (bac.type) {
            case BacteriaType.POISON:
                bac.Instagib();
                break;
            default:
                break;
        }
    }

    public void NotifySystem(Bacteria bacteria)
    {
        //Debug.Log("Notified of bacteria "+bacteria.name+ ".");
        AddBacteria(bacteria);
    }

    public void RecycleBacteria()
    {
        if (bacterias.Count == 0)
            return;
        foreach (var bac in bacterias.Values.ToArray())
        {
            bac.Instagib();
        }
    }

    public Bacteria AddBacteria(Bacteria bac){
        CheckOrganicImmunities(bac);
        CheckNonOrganicImmunities(bac);
        CheckSpecialImmunities(bac);
        if (!bac.gameObject.activeSelf) return null;
        if (!PublicPools.pools.ContainsKey(bac.name))
            return null;
        if (bacterias.ContainsKey(bac.name)){
            bacterias[bac.name].BacteriaIn();
            return bacterias[bac.name];
        }else{
            bacterias.Add(bac.name,bac);
            return bac;
        }
    }

    public Bacteria AddBacteria(string bacName){
        if (!PublicPools.pools.ContainsKey(bacName))
            return null;
        if (bacterias.ContainsKey(bacName)){
            bacterias[bacName].BacteriaIn();
            return bacterias[bacName];
        }else{
            Bacteria bac = PublicPools.pools[bacName].SendObject(gameObject).GetComponent<Bacteria>();
            CheckOrganicImmunities(bac);
            CheckNonOrganicImmunities(bac);
            CheckSpecialImmunities(bac);
            if (bacterias.ContainsKey(bacName)){
                bacterias[bacName].BacteriaIn();
                return bacterias[bacName];
            }
            bacterias.Add(bac.name,bac);
            return bac;
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
                    died = bacterias[bacKey].Degrade(immunityDamage*0.1f*stats.numericals["hostility"]*stats.numericals["allyDefense"]);
                }
                else if (bacterias[bacKey].character == BacteriaCharacter.POSITIVE){
                    died = bacterias[bacKey].Degrade(immunityDamage*0.4f*stats.numericals["hostility"]);
                } else
                {
                    //Debug.Log("degradation process: Damage-->"+immunityDamage+", Hostility-->"+stats.numericals["hostility"]);
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
