using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class MoneyBonus : MonoBehaviour
{
    private static int moneyBonus;

    public static void SetMoneyBonus(int num){
        moneyBonus = num;
    }

    public static void SetMoneyBonusGot(bool state){
        RunDataSave.rData.moneyBonusGot = state;
    }

    private void Start(){
        if (RunDataSave.rData.moneyBonusGot)
            return;
        PlayerInfo.SetMoney(moneyBonus);
        SetMoneyBonusGot(true);
    }
}
