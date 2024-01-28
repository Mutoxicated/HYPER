using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PassivePool : MonoBehaviour
{
    public static PassivePool pp;
    public static List<PopulativeInfo> enemyItems = new List<PopulativeInfo>();

    [SerializeField] private bool updater;
    [SerializeField] private bool incrementer;
    [SerializeField] private PassiveItemInfo[] passives;
    public Stats stats;
    public List<PassiveItem> myPassiveItems = new List<PassiveItem>();

    public void RecyclePassiveItems(){
        foreach(PassiveItem item in myPassiveItems){
            PublicPools.pools[item.gameObject.name].ReattachImmediate(item.gameObject);
        }
        myPassiveItems.Clear();
    }

    public static void UpdateRunDataPassives(){
        RunDataSave.rData.enemyPassives = enemyItems.ToList();
    }

    public void AddPassive(){
        string passiveName = passives[SeedGenerator.random.Next(0,passives.Length)].itemName;
        foreach (PopulativeInfo pi in enemyItems){
            if (pi.name == passiveName){
                pi.population++;
                return;
            }
        }
        enemyItems.Add(new PopulativeInfo(passiveName,1));
    }

    private void OnEnable(){
        if (updater){
            pp = this;
            if (RunDataSave.rData.getPassives){
                RunDataSave.rData.getPassives = false;
            }else{
                enemyItems = RunDataSave.rData.enemyPassives;
            }
            return;
        }
        if (incrementer){
            AddPassive();
            return;
        }
        if (enemyItems.Count == 0)
            return;
        foreach (PopulativeInfo pi in enemyItems){
            PassiveItem ak = PublicPools.pools[pi.name.ToUpper().Replace(" ","_")].SendObject(gameObject).GetComponent<PassiveItem>();
            ak.origin = this;
            myPassiveItems.Add(ak);
            ak.population = pi.population;
        }
        foreach(PassiveItem item in myPassiveItems){
            item.gameObject.SetActive(true);
        }
    }
}
