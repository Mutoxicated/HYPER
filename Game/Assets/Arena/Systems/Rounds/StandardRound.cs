using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StandardRound : MonoBehaviour, IRound
{
    public static readonly float initEnemySpawnInterval = 4f;
    public static readonly float initDuration = 100f;

    public Difficulty diff;
    public GameObject beamInstance;
    public TMP_Text roundText;

    public static float enemySpawnInterval = initEnemySpawnInterval;
    public static float duration = initDuration;

    [SerializeField] private OnInterval spawnInterval;
    [SerializeField] private OnInterval durationInterval;

    private void Start(){
        Difficulty.rounds++;
        roundText.text = Difficulty.rounds.ToString();
        spawnInterval.ChangeInterval(enemySpawnInterval);
        durationInterval.ChangeInterval(duration);
        PreSpawnEnemies();

        ProgressDifficulty();
    }

    public void SpawnEnemy(){
        Instantiate(diff.enemyPool[Random.Range(0,diff.enemyPool.Count)],
                    diff.spawnPoints[Random.Range(0,diff.spawnPoints.Length)].transform.position, 
                    Quaternion.identity);
    }

    public void PreSpawnEnemies(){
        int spawnAmount = Mathf.RoundToInt(duration/enemySpawnInterval*0.25f);
        for (int i = 0; i < spawnAmount; i++){
            SpawnEnemy();
        }
    }

    public void EndRound()
    {
        beamInstance.SetActive(true);
        Destroy(gameObject);
    }

    public void ProgressDifficulty()
    {
        enemySpawnInterval = initEnemySpawnInterval*diff.t;
        duration = initDuration*diff.t;
    }
}
