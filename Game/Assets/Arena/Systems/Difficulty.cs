using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour 
{
    //Info enemies will use
    public static Transform player;
    public static List<GameObject> enemies = new List<GameObject>();

    public int entitySpawnerPopulation = 1;
    public int wavePopulation = 1;
    public int sequencePopulation = 2;
    public int spawnerPopulation = 1;
    public float allyBacteriaChance = 100f;

    public List<GameObject> enemyPool;

    public static GameObject FindClosestEnemy(Transform trans)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in enemies)
        {
                    Debug.Log(go.name);
            Vector3 diff = go.transform.position - trans.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < 1f)
                continue;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    

    private void Awake(){
        player = GameObject.FindWithTag("Player").transform;
    }
}
