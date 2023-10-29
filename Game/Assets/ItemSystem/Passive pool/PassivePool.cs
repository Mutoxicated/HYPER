using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePool : MonoBehaviour
{
    public static List<string> enemyItems = new List<string>();
    public static List<string> playerItems = new List<string>();

    public Stats stats;
    public DeathFor entityType;
    private List<PassiveItem> myPassiveItems = new List<PassiveItem>();

    private void AddPassiveItem(string poolID){
        PassiveItem ak = PublicPools.pools[poolID].SendObject(gameObject).GetComponent<PassiveItem>();
        if (myPassiveItems.Contains(ak)){
            myPassiveItems[myPassiveItems.IndexOf(ak)].population++;
        }else{
            myPassiveItems.Add(ak);
        }
    }

    public void RecyclePassiveItems(){
        foreach(PassiveItem item in myPassiveItems){
            PublicPools.pools[item.gameObject.name].ReattachImmediate(item.gameObject);
        }
    }

    private void OnEnable(){
        if (entityType == DeathFor.PLAYER){
            if (enemyItems.Count == 0)
                return;
            foreach (string poolID in playerItems){
                AddPassiveItem(poolID);
            }
        }else{
            if (playerItems.Count == 0)
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
