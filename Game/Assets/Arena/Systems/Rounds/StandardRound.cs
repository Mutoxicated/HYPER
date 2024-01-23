using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StandardRound : MonoBehaviour, IRound
{
    public static readonly float initEnemySpawnInterval = 4f;
    public static readonly float initDuration = 40f;
    public static readonly float initDifficultyMod = 4f;

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
                    Difficulty.spawnPoints[Random.Range(0,Difficulty.spawnPoints.Count)].transform.position, 
                    Quaternion.identity);
    }

    public void PreSpawnEnemies(){
        int spawnAmount = Mathf.RoundToInt(duration/enemySpawnInterval*0.5f);
        for (int i = 0; i < spawnAmount; i++){
            SpawnEnemy();
        }
    }

    public void EndRound()
    {
        Difficulty.roundFinished = true;
        beamInstance.SetActive(true);
        Destroy(gameObject);
    }

    public void ProgressDifficulty()
    {
        enemySpawnInterval = Mathf.Lerp(initEnemySpawnInterval,initEnemySpawnInterval*initDifficultyMod,diff.asymT);
        duration = initDuration*diff.linearT;
    }
}
