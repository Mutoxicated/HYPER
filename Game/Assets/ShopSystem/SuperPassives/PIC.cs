using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PIC : MonoBehaviour
{
    [SerializeField] private Image cspImage;
    [SerializeField] private Slot[] slots; 
    
    [SerializeField] private PassiveItemInfo[] sps;

    private float number;
    private SuperPassive currentSuperPassive;
    private Sprite empty;

    private PassiveItemInfo GetPassiveInfoByName(string name){
        foreach (PassiveItemInfo pii in sps){
            if (pii.itemName.ToUpper().Replace(" ","_") == name){
                return pii;
            }
        }
        return null;
    }

    private void EvaluateOperators(PassiveItem pi){
        Debug.Log(pi._operator);
        switch (pi._operator){
            case PassiveOperators.ADD:
                number += pi.number;
                break;
            case PassiveOperators.DIVIDE:
                number /= pi.number;
                break;
            case PassiveOperators.MODULO:
                number %= pi.number;
                break;
            case PassiveOperators.MULTIPLY:
                number *= pi.number;
                break;
            case PassiveOperators.POWER:
                number = Mathf.Pow(number,pi.number);
                break;    
            case PassiveOperators.SUBTRACT:
                number -= pi.number;
                break;    
            default:
                break;
        }
    }

    private void EvaluateStructure(){
        number = 0f;
        //Debug.Log("--START--");
        foreach (Slot slot in slots){
            if (slot.GetSO() == null) {
                currentSuperPassive = null;
                cspImage.sprite = empty;
                return;
            }
            EvaluateOperators(slot.GetSO().pip.GetCurrentPassive());
        }
        number = Mathf.Abs(Mathf.RoundToInt(number));

        //Debug.Log("Number got initially: "+number);

        if (number > SuperPassivePool.GetSuperPassiveCount()){
            float times = Mathf.Floor(number/SuperPassivePool.GetSuperPassiveCount());
            number -= SuperPassivePool.GetSuperPassiveCount()*times;
        }

        //Debug.Log("Number got post-processing: "+number);

        currentSuperPassive = SuperPassivePool.GetPassiveByIndex(Mathf.RoundToInt(number));
        cspImage.sprite = GetPassiveInfoByName(currentSuperPassive.gameObject.name).itemImage;
        //Debug.Log("Current Super Passive:" + Mathf.RoundToInt(number));
    }

    private void Start(){
        empty = cspImage.sprite;
    }

    private void Update(){
        EvaluateStructure();
    }
}
