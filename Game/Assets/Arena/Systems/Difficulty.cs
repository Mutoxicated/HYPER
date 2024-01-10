using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour 
{
    //Info enemies will use
    public static Transform player;
    public static List<GameObject> enemies = new List<GameObject>();
    public static ExtraUtils utils;

    public int wavePopulation = 1;
    public int sequencePopulation = 2;
    public int spawnerPopulation = 1;
    public int entitySpawnerPopulation = 1;
    public float allyBacteriaChance = 100f;

    public List<GameObject> enemyPool;

    public static GameObject FindClosestEnemy(Transform trans, float distTol)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in enemies)
        {
            //Debug.Log(go.name);
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
        if (distance > distTol){
            return null;
        }
        return closest;
    }

    private static GameObject[] SortGameObjects(GameObject[] objects, Transform trans)
    {
        GameObject temp;
        float distTemp;
        GameObject[] sortedObjects = objects;
        List<float> distances = new List<float>();

        foreach (GameObject go in objects){
            Vector3 diff = go.transform.position - trans.position;
            distances.Add(diff.sqrMagnitude);
        }

        for (int i = 0; i <= objects.Length - 1; i++)
        {
            for (int j = i + 1; j < objects.Length; j++)
            {
                if (distances[i] > distances[j])
                {
                    //distance
                    distTemp = distances[i];
                    distances[i] = distances[j];
                    distances[j] = distTemp;
                    //gameobject
                    temp = sortedObjects[i];
                    sortedObjects[i] = sortedObjects[j];
                    sortedObjects[j] = temp;
                }
            }
        }
        return sortedObjects;
    }
    
    public static GameObject[] FindClosestEntities(Transform trans){
        List<GameObject> gos = enemies;
        gos.Add(player.gameObject);
        return SortGameObjects(gos.ToArray(), trans);
    }

    private void Awake(){
        player = GameObject.FindWithTag("Player").transform;
        utils = GameObject.FindWithTag("ExtraUtils").GetComponent<ExtraUtils>();
    }
}
