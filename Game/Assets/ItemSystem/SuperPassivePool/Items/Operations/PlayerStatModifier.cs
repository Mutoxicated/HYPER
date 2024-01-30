using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerStatModifier : MonoBehaviour
{
    [SerializeField] private SuperPassive sp;
    [SerializeField] private string[] numericals;
    [SerializeField] private float[] numericalIncrements;

    [SerializeField] private float shieldAmount = -1;
    [SerializeField] private float shieldStep = -1;

    private int shieldsAdded = 0;

    private void GiveShields(){
        if (shieldAmount < 0) return;
        shieldAmount += shieldStep;
        int shieldsToAdd = Mathf.RoundToInt(shieldAmount)-shieldsAdded;
        if (shieldsToAdd == 0) return;
        shieldsAdded = Mathf.RoundToInt(shieldAmount);
        PlayerInfo.GetGun().stats.AddShield(shieldsToAdd);
    }

    private void Increment(int num){
        GiveShields();
        if (numericals.Length == 0)
            return;
        for (int i = 0; i < numericals.Length; i++)
        {
            PlayerInfo.GetGun().stats.numericals[numericals[i]] += numericalIncrements[i]*num;
        }
    }

    private void Start()
    {
        sp.subs.Add(Increment);
    }
}
