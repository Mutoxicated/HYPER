using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraUtils : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    private Stopwatch stopwatch = new Stopwatch();

    public bool spawnThem;
    public GameObject[] entitiesToSpawn;
    private GameObject[] spawnPoints;

    private ButtonInput dbreak = new ButtonInput("DebugBreak");
    
    public void StopTime(){
        stopwatch.Start();
        Time.timeScale = 0f;
        screen.SetActive(true);
    }

    private void Start(){
        if (!spawnThem)
            return;
        if (entitiesToSpawn.Length == 0)
            return;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for (int i = 0; i < entitiesToSpawn.Length;i++){
            Instantiate(entitiesToSpawn[i],
            spawnPoints[i].transform.position, 
            spawnPoints[i].transform.rotation);
        }
    }

    private void Update(){
        dbreak.Update();
        if (dbreak.GetInputDown()){
            UnityEngine.Debug.Break();
        }
        if (stopwatch.ElapsedMilliseconds > 214){
            stopwatch.Reset();
            Time.timeScale = 1f;
            screen.SetActive(false);
        }
    }
}
