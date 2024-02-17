using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class StandardRound : MonoBehaviour, IRound
{
    public static readonly float initEnemySpawnInterval = 3.5f;
    public static readonly float initDuration = 12f;
    public static readonly float initDifficultyMod = 4f;
    public static readonly float initValue = 5;

    public Difficulty diff;
    public GameObject beamInstance;
    public TMP_Text roundText;

    [HideInInspector] public float enemySpawnInterval = initEnemySpawnInterval;
    [HideInInspector] public float duration = initDuration;

    [SerializeField] private OnInterval spawnInterval;
    [SerializeField] private OnInterval durationInterval;

    private float value = initValue;
    private float currentValue = initValue;
    private int intervalsHappened = 0;
    private int rando = 0;

    private void Awake(){
        Difficulty.StepRound();
    }

    private void Start(){
        roundText.text = Difficulty.rounds.ToString();
        ProgressDifficulty();
        spawnInterval.ChangeInterval(enemySpawnInterval);
        durationInterval.ChangeInterval(duration);
        PreSpawnEnemies();
    }

    public void SpawnEnemyExtra(){
        int key = diff.organizedEnemyPool.ElementAt(SeedGenerator.random.Next(0,diff.organizedEnemyPool.Count)).Key;
        Instantiate(diff.organizedEnemyPool[key][SeedGenerator.random.Next(0,diff.organizedEnemyPool[key].Count)],
                    Difficulty.spawnPoints[SeedGenerator.random.Next(0,Difficulty.spawnPoints.Count)].transform.position, 
                    Quaternion.identity);
    }

    private void SpawnEnemy(int former, int latter){
        for (int i = 0; i < former; i++){
            Instantiate(diff.organizedEnemyPool[latter][SeedGenerator.random.Next(0,diff.organizedEnemyPool[latter].Count)],
                Difficulty.spawnPoints[SeedGenerator.random.Next(0,Difficulty.spawnPoints.Count)].transform.position, 
                Quaternion.identity);
        }
    }

    private void GetRando(){
        if (rando == 0){
            rando = SeedGenerator.random.Next(2,Mathf.RoundToInt(Mathf.Clamp(currentValue,2,9999999)));
        }
        if (!diff.organizedEnemyPool.ContainsKey(rando)){
            rando--;
            GetRando();
        }
    }

    public void SpawnEnemyInterval(){
        if (rando == -1){
            currentValue = value;
            rando = 0;
            return;
        }
        intervalsHappened++;

        GetRando();

        int remainder = Mathf.FloorToInt(Mathf.RoundToInt(currentValue)%rando);
        int former = Mathf.FloorToInt(Mathf.RoundToInt(currentValue)/rando);

        if (SeedGenerator.random.Next(0,100) >= 50){
            SpawnEnemy(former,rando);
        }else{
            SpawnEnemy(former,rando);
        }
        SpawnEnemy(remainder,1);
        currentValue -= value/2f;
        rando = 0;

        if (currentValue <= 0f){
            currentValue = 0f;
            rando = -1;
        }
    }

    public void PreSpawnEnemies(){
        int spawnAmount = Mathf.RoundToInt(duration/enemySpawnInterval);
        for (int i = 0; i < spawnAmount; i++){
            SpawnEnemyExtra();
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

    public void ProgressDifficulty()
    {
        enemySpawnInterval = Mathf.Lerp(initEnemySpawnInterval,initEnemySpawnInterval*initDifficultyMod,diff.asymT);
        duration = initDuration*diff.linearT;
        value = initValue*diff.linearT;
    }

    public void EvaluateMoneyBonus(){
        int money = 0;
        money = Mathf.RoundToInt(PlayerInfo.GetScore()/600);
        MoneyBonus.SetMoneyBonus(money);
    }
}
