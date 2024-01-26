using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePool : MonoBehaviour
{
    public static Dictionary<string,int> enemyItems = new Dictionary<string,int>(){
    };
    public static Dictionary<string,int> playerItems = new Dictionary<string,int>();

    public Stats stats;
    public DeathFor entityType;
    public List<PassiveItem> myPassiveItems = new List<PassiveItem>();

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
            foreach (string poolID in playerItems.Keys){
                PassiveItem ak = PublicPools.pools[poolID].SendObject(gameObject).GetComponent<PassiveItem>();
                ak.origin = this;
                myPassiveItems.Add(ak);
                ak.population = playerItems[poolID];
            }
        }else{
            if (enemyItems.Count == 0)
                return;
            foreach (string poolID in enemyItems.Keys){
                PassiveItem ak = PublicPools.pools[poolID].SendObject(gameObject).GetComponent<PassiveItem>();
                ak.origin = this;
                myPassiveItems.Add(ak);
                ak.population = enemyItems[poolID];
            }
        }
        foreach(PassiveItem item in myPassiveItems){
            item.gameObject.SetActive(true);
        }
    }
}
