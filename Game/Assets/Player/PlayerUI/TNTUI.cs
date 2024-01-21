using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TNTUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tntCapacity;
    [SerializeField] private TMP_Text currentTNTAmount;
    
    private void UpdateElements(){
        tntCapacity.text = PlayerInfo.GetGun().stats.numericals["maxCapacitor1"].ToString();
        currentTNTAmount.text = PlayerInfo.GetGun().stats.numericals["capacitor1"].ToString();
    }

    private void Start(){
        UpdateElements();
    }

    private void Update(){
        UpdateElements();
    }
}
