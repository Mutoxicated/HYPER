using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Numerical;

public class TNTUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tntCapacity;
    [SerializeField] private TMP_Text currentTNTAmount;
    
    private void UpdateElements(){
        tntCapacity.text = PlayerInfo.GetGun().stats.numericals[MAX_CAPACITOR_1].ToString();
        currentTNTAmount.text = PlayerInfo.GetGun().stats.numericals[CAPACITOR_1].ToString();
    }

    private void Start(){
        UpdateElements();
    }

    private void Update(){
        UpdateElements();
    }
}
