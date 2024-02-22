using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;

public class MoneyBonus : MonoBehaviour
{
    private static int moneyBonus;

    [SerializeField] private TMP_Text money;

    public static void SetMoneyBonus(int num){
        moneyBonus = num;
        RunDataSave.rData.moneyBonus = moneyBonus;
    }

    public static void SetMoneyBonusGot(bool state){
        RunDataSave.rData.moneyBonusGot = state;
    }

    public static int GetMoneyBonus(){
        return moneyBonus;
    }

    private void Start(){
        Debug.Log("Getting the money bonus from data, reading: "+RunDataSave.rData.moneyBonus);
        if (RunDataSave.rData.moneyBonus < 0){
            RunDataSave.rData.moneyBonus = moneyBonus;
        }else{
            moneyBonus = RunDataSave.rData.moneyBonus;
        }
        money.text = moneyBonus+"* !!!";
        if (RunDataSave.rData.moneyBonusGot)
            return;
        PlayerInfo.SetMoney(moneyBonus);
        SetMoneyBonusGot(true);
    }
}
