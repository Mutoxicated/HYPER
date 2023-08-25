using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicPools : MonoBehaviour
{
    public static Dictionary<string, ObjectPoolManager> pools = new Dictionary<string, ObjectPoolManager>();
    private static bool spawned = false;

    private void Awake()
    {
        if (!spawned)
        {
            DontDestroyOnLoad(gameObject);
            spawned = true;
        }
    }

    public static bool PoolsExists(string ID)
    {
        return pools.ContainsKey(ID);
    }
}
