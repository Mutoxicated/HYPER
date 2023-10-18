using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraUtils : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    private Stopwatch stopwatch = new Stopwatch();
    
    public void StopTime(){
        stopwatch.Start();
        Time.timeScale = 0f;
        screen.SetActive(true);
    }

    private void Update(){
        if (stopwatch.ElapsedMilliseconds > 214){
            stopwatch.Reset();
            Time.timeScale = 1f;
            screen.SetActive(false);
        }
    }
}
