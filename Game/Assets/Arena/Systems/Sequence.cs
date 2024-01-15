using System.Collections.Generic;
using UnityEngine;

public class Sequence : MonoBehaviour
{
    private static Difficulty diff;
    public static GameObject[] spawnPoints;

    private List<Spawner> spawners = new List<Spawner>();
    private const float spawnInterval = 0.2f;

    private List<GameObject> entities = new List<GameObject>();
    private float t = 0f;
    private int spawns = 0;
    private bool sequenceEnded = false;

    private void Start()
    {
        if (diff == null)
            diff = GameObject.FindWithTag("Difficulty").GetComponent<Difficulty>();
        if (spawnPoints == null)
            spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for (int i = 0; i < diff.spawnerPopulation; i++)
        {
            spawners.Add(new Spawner(spawnPoints[Random.Range(0, spawnPoints.Length)].transform));
        }
    }

    private void Update()
    {
        t += Time.deltaTime;
        CalculateEntities();
        if (sequenceEnded)
            return;
        if (t >= spawnInterval)
        {
            spawns++;
            for (int i = 0; i < spawners.Count; i++)
            {
                GameObject instance = Instantiate(diff.enemyPool[Random.Range(0, diff.enemyPool.Count)], spawners[i].coords, spawners[i].rotation);
                entities.Add(instance);
            }
        }
        if (spawns >= diff.entitySpawnerPopulation)
        {
            sequenceEnded = true;
        }
    }

    public static void RemoveSpawnPoints()
    {
        spawnPoints = null;
    }

    public void EndSequence()
    {
        Destroy(gameObject);
    }

    private void CalculateEntities()
    {
        if (entities.Count == 0)
            return;
        foreach (var entity in entities)
        {
            if (entity == null)
            {
                entities.Remove(entity);
                break;
            }
        }
    }

    public int EnemiesLeft()
    {
        return entities.Count;
    }

    public bool SequenceEnded()
    {
        return sequenceEnded;
    }
}
