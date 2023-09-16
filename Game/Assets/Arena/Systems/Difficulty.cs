using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour 
{
    public int entitySpawnerPopulation = 1;
    public int wavePopulation = 1;
    public int sequencePopulation = 2;
    public int spawnerPopulation = 1;
    public float allyBacteriaChance = 100f;

    public List<GameObject> enemyPool;
}
