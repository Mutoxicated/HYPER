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
    }

    public static void SetMoneyBonusGot(bool state){
        RunDataSave.rData.moneyBonusGot = state;
    }

    private void Start(){
        if (RunDataSave.rData.moneyBonusGot)
            return;
        PlayerInfo.SetMoney(moneyBonus);
        money.text = moneyBonus+"* !!!";
        SetMoneyBonusGot(true);
    }
}
