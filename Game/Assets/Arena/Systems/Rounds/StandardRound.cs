using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StandardRound : MonoBehaviour, IRound
{
    public static readonly float initEnemySpawnInterval = 3.5f;
    public static readonly float initEnemySpawnIntervalDecrease = 0.1f;
    public static readonly float initDuration = 14f;
    public static readonly float initDifficultyMod = 4f;
    public static readonly float initValue = 13;

    public Difficulty diff;
    public GameObject beamInstance;
    public TMP_Text roundText;

    [HideInInspector] public float enemySpawnInterval = initEnemySpawnInterval;
    [HideInInspector] public float duration = initDuration;

    [SerializeField] private OnInterval spawnInterval;
    [SerializeField] private OnInterval durationInterval;

    private float value = initValue;
    private float currentValue;

    public void IncrementCurrentValue() {
        currentValue += 0.25f;
    }

    private void Awake(){
        Difficulty.StepRound();
    }

    private void Start(){
        roundText.text = Difficulty.rounds.ToString();
        currentValue = value;
        UpdateDifficulty();
        spawnInterval.ChangeMinMaxInterval(new Vector2(
            spawnInterval.MinMax.x+enemySpawnInterval, 
            spawnInterval.MinMax.y+enemySpawnInterval));

        spawnInterval.RandomizeInterval();
        durationInterval.ChangeInterval(duration);
        PreSpawnEnemies();
    }

    public void SpawnEnemy(){
        if (currentValue <= 0f) {
            return;
        } 
        int key = diff.GetRandomEnemyListKey();
        currentValue -= key;
        Instantiate(diff.organizedEnemyPool[key][SeedGenerator.random.Next(0,diff.organizedEnemyPool[key].Count)],
                    Difficulty.spawnPoints[SeedGenerator.random.Next(0,Difficulty.spawnPoints.Count)].transform.position, 
                    Quaternion.identity);
    }

    public void SpawnEnemies() {
        
    }

    public void PreSpawnEnemies(){
        int platformCount = PlatformGenerator.PG.GetPlatformCount();
        Debug.Log(platformCount);
        int spawnAmount = Mathf.CeilToInt(platformCount*diff.asymT);
        for (int i = 0; i < spawnAmount; i++){
            SpawnEnemy();
        }
    }

    public void EndRound()
    {
        EvaluateMoneyBonus();
        Difficulty.roundFinished = true;
        beamInstance.SetActive(true);
        spawnInterval.enabled = false;
        Destroy(gameObject);
    }

    public void UpdateDifficulty()
    {
        enemySpawnInterval = Mathf.Lerp(initEnemySpawnInterval,initEnemySpawnInterval*initDifficultyMod,diff.asymT);
        duration = initDuration*diff.linearT;
        value = initValue*diff.linearT;
    }

    public void EvaluateMoneyBonus(){
        int money;
        money = Mathf.RoundToInt(PlayerInfo.GetScore()/600);
        Debug.Log("Money Bonus: "+money);
        MoneyBonus.SetMoneyBonus(money);
    }
}
