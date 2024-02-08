using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timer;

    private int minutes = 0;
    private int seconds = 0;

    private float time;

    private string StringifyValue(int num){
        if (num < 10){
            return "0"+num;
        }else{
            return num.ToString();
        }
    }

    private void Update(){
        time += Time.deltaTime;
        if (time >= 1f){
            seconds++;
            if (seconds == 60){
                seconds = 0;
                minutes++;
            }
            timer.text = StringifyValue(minutes)+":"+StringifyValue(seconds);
            time = 0f;
        }
    }
}
