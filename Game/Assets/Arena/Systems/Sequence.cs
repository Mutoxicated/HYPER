using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class Sequence : MonoBehaviour
{
    private static Difficulty diff;

    private int spawner_amount;
    public int spawn_times;
    public float spawn_interval;

    private List<GameObject> entities = new List<GameObject>();
    private float t = 0f;
    private int current_times = 0;
    private bool sequenceEnded = false;

    private void Start()
    {
        if (diff == null)
            diff = GameObject.FindWithTag("Difficulty").GetComponent<Difficulty>();
    }

    private void Update()
    {
        t += Time.deltaTime;
        if (t >= spawn_interval)
        {
            current_times++;
            Debug.Log("Enemy Spawned at location: ");
        }
        if (current_times >= spawn_times)
        {
            sequenceEnded = true;
        }

        CalculateEntities();
    }

    private void CalculateEntities()
    {
        foreach (var entity in entities)
        {
            if (entity.IsDestroyed())
            {
                entities.Remove(entity);
            }
        }
    }

    private int EnemiesLeft()
    {
        return entities.Count;
    }
}
