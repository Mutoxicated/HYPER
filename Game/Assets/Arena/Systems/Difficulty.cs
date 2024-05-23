using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Difficulty : MonoBehaviour 
{
    public static bool roundFinished = false;
    public static int rounds = 0;
    public static Transform player;
    public static List<GameObject> enemies = new List<GameObject>();
    public static ExtraUtils utils;
    public static List<GameObject> spawnPoints = new List<GameObject>();
    public static readonly float difficultyScale = 1f;
    public static readonly float startOffset = 2f;

    public float linearT = 1f;
    public float asymT = 0f;
    public int wavePopulation = 1;
    public int sequencePopulation = 2;
    public int spawnerPopulation = 1;
    public int entitySpawnerPopulation = 1;
    public float allyBacteriaChance = 100f;

    public List<EnemyInfo> enemyPool;
    public Dictionary<int, List<GameObject>> organizedEnemyPool = new Dictionary<int, List<GameObject>>();
    public List<int> enemyListKeys = new List<int>();

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

    private static List<GameObject> SortGameObjects(List<GameObject> objects, Transform trans, float maxDist)
    {
        GameObject temp;
        float distTemp;
        var sortedObjects = objects;
        List<float> distances = new List<float>();

        foreach (GameObject go in objects.ToArray()){
            Vector3 diff = go.transform.position - trans.position;
            if (diff.sqrMagnitude >= maxDist) {
                objects.Remove(go);
                continue;
            }
            distances.Add(diff.sqrMagnitude);
        }

        for (int i = 0; i <= objects.Count - 1; i++)
        {
            for (int j = i + 1; j < objects.Count; j++)
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
    
    public static GameObject[] FindClosestEntities(Transform trans, float range){
        List<GameObject> gos = enemies;
        return SortGameObjects(gos, trans, range).ToArray();
    }

    public static void StepRound(){
        rounds++;
        RunDataSave.rData.rounds = rounds;
    }

    public int GetRandomEnemyListKey() {
        return enemyListKeys[(int)SeedGenerator.NextFloat(0,enemyListKeys.Count-1)];
    }

    private void DevelopSuperPassives(){
        SuperPassivePool.DevelopPassiveByName("JUICE");
        SuperPassivePool.DevelopPassiveByName("POTION");
        SuperPassivePool.DevelopPassiveByName("FLASH_DRIVE");
        SuperPassivePool.DevelopPassiveByName("GEE");
        SuperPassivePool.DevelopPassiveByName("Ferocity");
    }

    private void OrganizeEnemyPool(){
        foreach (EnemyInfo ei in enemyPool){
            if (!organizedEnemyPool.ContainsKey(ei.importance)){
                organizedEnemyPool.Add(ei.importance,new List<GameObject>(){ei.enemyPrefab});
                enemyListKeys.Add(ei.importance);
            }else{
                organizedEnemyPool[ei.importance].Add(ei.enemyPrefab);
            }
        }
    }

    public void DestroyAll(){
        foreach (GameObject enemy in enemies.ToArray()){
            Destroy(enemy);
        }
    }

    private void Awake(){
        OrganizeEnemyPool();
        rounds = RunDataSave.rData.rounds;
        roundFinished = false;
        asymT = startOffset+Mathf.Sqrt(rounds/0.2f) * difficultyScale;
        //Debug.Log("rounds: "+rounds);
        linearT = startOffset+rounds*0.2f * difficultyScale;
        //Debug.Log("linearT: "+linearT);
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();
        player = GameObject.FindWithTag("Player").transform;
        utils = GameObject.FindWithTag("ExtraUtils").GetComponent<ExtraUtils>();
    }

    private void Start(){
        DevelopSuperPassives();
    }
}
