using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerStatModifier : MonoBehaviour
{
    [SerializeField] private string[] numericals;
    [SerializeField] private float[] numericalIncrements;

    [SerializeField] private int shieldAmount = -1;

    private void GiveShields(){
        if (shieldAmount <= 0) return;

        PlayerInfo.GetGun().stats.AddShield(shieldAmount);
    }

    private void Increment(){
        GiveShields();
        if (numericals.Length == 0)
            return;
        for (int i = 0; i < numericals.Length; i++)
        {
            PlayerInfo.GetGun().stats.numericals[numericals[i]] += numericalIncrements[i];
        }
    }

    private void Start()
    {
        Increment();
    }

    private void Develop(){
        Increment();
    }
}
