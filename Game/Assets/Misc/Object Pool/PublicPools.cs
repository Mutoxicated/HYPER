using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicPools : MonoBehaviour
{
    public static bool spawned = true;
    public static Dictionary<string, ObjectPoolManager> pools = new Dictionary<string, ObjectPoolManager>();
    private void Awake()
    {
        if (!spawned){
            pools.Clear();
            DontDestroyOnLoad(gameObject.transform.root);
            spawned = true;
        }
    }

    public static void SetSpawn(bool state){
        spawned = state;
    }

    public static void RetrieveAllObjectsToPools(){
        foreach (var poolname in pools.Keys){
            foreach (Transform trans in pools[poolname].transform){
                if (trans == null)
                    continue;
                if (!trans.gameObject.activeSelf)
                    continue;
                pools[poolname].ReattachImmediate(trans.gameObject);
            }
        }
    }

    public static bool PoolsExists(string ID)
    {
        return pools.ContainsKey(ID);
    }
}
