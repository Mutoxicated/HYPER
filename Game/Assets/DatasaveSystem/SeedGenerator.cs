using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeedGenerator : MonoBehaviour
{
    public static System.Random random;

    void Awake()
    {
        if (RunDataSave.rData.seed == -1){
            RunDataSave.rData.seed = UnityEngine.Random.Range(0,999999999);
        }
        random = new System.Random(RunDataSave.rData.seed);
    }

    public static float NextFloat(float min, float max){
        int minINT = Mathf.RoundToInt(min*100f);
        int maxINT = Mathf.RoundToInt(max*100f);
        return random.Next(minINT,maxINT)/100f;
    }
}
