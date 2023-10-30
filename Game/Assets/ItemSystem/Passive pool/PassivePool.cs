using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePool : MonoBehaviour
{
    public static List<string> enemyItems = new List<string>(){
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE",
        "FEEBLE"
    };
    public static List<string> playerItems = new List<string>();

    public Stats stats;
    public DeathFor entityType;
    public List<PassiveItem> myPassiveItems = new List<PassiveItem>();

    private void AddPassiveItem(string poolID){
        foreach (PassiveItem item in myPassiveItems){
            if (item.name == poolID){
                Debug.Log("Es hat schon dieses Item.");
                myPassiveItems[myPassiveItems.IndexOf(item)].population++;
                return;
            }
        }
        PassiveItem ak = PublicPools.pools[poolID].SendObject(gameObject).GetComponent<PassiveItem>();
        ak.origin = this;
        myPassiveItems.Add(ak);
    }

    public void RecyclePassiveItems(){
        foreach(PassiveItem item in myPassiveItems){
            PublicPools.pools[item.gameObject.name].ReattachImmediate(item.gameObject);
        }
        myPassiveItems.Clear();
    }

    private void OnEnable(){
        if (entityType == DeathFor.PLAYER){
            if (playerItems.Count == 0)
                return;
            foreach (string poolID in playerItems){
                AddPassiveItem(poolID);
            }
        }else{
            if (enemyItems.Count == 0)
                return;
            foreach (string poolID in enemyItems){
                AddPassiveItem(poolID);
            }
        }
        foreach(PassiveItem item in myPassiveItems){
            item.gameObject.SetActive(true);
        }
    }
}
