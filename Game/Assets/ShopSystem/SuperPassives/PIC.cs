using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PIC : MonoBehaviour
{
    public static PIC PICVier;
    public static PIC PICTri;
    public static PIC PICDuo;

    [SerializeField] private PassiveItemManager pim;
    [SerializeField] private Image cspImage;
    [SerializeField] private TMP_Text iterations;
    [SerializeField] private Slot[] slots; 
    
    [SerializeField] private PassiveItemInfo[] sps;

    private float number;
    private SuperPassive currentSuperPassive;
    private Sprite empty;

    private List<PassiveItemInfo> currentPiis = new List<PassiveItemInfo>();
    private List<SlotOccupant> oldSlotOccupants = new List<SlotOccupant>();
    private int index = 0;

    public static void UpdateRunDatas(){
        PICVier.UpdateRunData();
        PICTri.UpdateRunData();
        PICDuo.UpdateRunData();
    }

    public static void SetCurrentPIIS(){

        PICVier.SetCurrentPiis();
        PICTri.SetCurrentPiis();
        PICDuo.SetCurrentPiis();
    }


    public PassiveItemManager GetPIM(){
        return pim;
    }

    public void SetCurrentPiis(){
        for (int i = 0; i < slots.Length; i++){
            Debug.Log("SETTING PIIS THROUGH SLOTS");
            Debug.Log(slots[i].GetSO());
            if (slots[i].GetSO() == null) {
                currentPiis.Add(null);
                continue;
            }
            Debug.Log("PASSIVE NAME: "+pim.GetPassiveItemInfoByPassiveName(slots[i].GetSO().pip.GetCurrentPassive().gameObject.name).name);
            currentPiis[i] = pim.GetPassiveItemInfoByPassiveName(slots[i].GetSO().pip.GetCurrentPassive().gameObject.name);
        }
    }

    public List<PassiveItemInfo> GetCurrentPIIs(){
        return currentPiis;
    }

    public void UpdateRunData(){
        enabled = false;
        if (slots.Length == 4){
            RunDataSave.rData.PICVierPassiveItems = currentPiis;
        }else if (slots.Length == 3){
            RunDataSave.rData.PICTriPassiveItems = currentPiis;
        }else if (slots.Length == 2){
            RunDataSave.rData.PICDuoPassiveItems = currentPiis;
        }else{
            Debug.Log("Uhhhh slots length doesnt match up. Slot count: "+slots.Length);
        }  
    }

    private void RegenerateSlots(){
        if (slots.Length == 4){
            currentPiis = RunDataSave.rData.PICVierPassiveItems;
        }else if (slots.Length == 3){
            currentPiis = RunDataSave.rData.PICTriPassiveItems;
        }else if (slots.Length == 2){
            currentPiis = RunDataSave.rData.PICDuoPassiveItems;
        }else{
            Debug.Log("Uhhhh slots length doesnt match up. Slot count: "+slots.Length);
        }   
        for (int i = 0; i < slots.Length; i++){
            if (currentPiis[i] == null){
                Destroy(slots[i].GetSO().gameObject);
                slots[i].SetSO(null);
                continue;
            }
            slots[i].GetSO().pip.SetImageSprite(currentPiis[i].itemImage);
            slots[i].GetSO().pip.SetTitle(currentPiis[i].itemName);
            slots[i].GetSO().pip.SetCurrentPassive(currentPiis[i].item);
        }
    }

    private PassiveItemInfo GetPassiveInfoByName(string name){
        foreach (PassiveItemInfo pii in sps){
            if (pii.itemName.ToUpper().Replace(" ","_") == name){
                return pii;
            }
        }
        return null;
    }

    private void EvaluateOperators(PassiveItem pi){
        //Debug.Log(pi._operator);
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
        index = 0;
        //Debug.Log("--START--");
        foreach (Slot slot in slots){
            if (slot.GetSO() == null) {
                if (currentSuperPassive != null && currentSuperPassive.GetDevelopState()){
                    currentSuperPassive.SetDevelopState(false);
                    currentSuperPassive = null;
                }
                cspImage.sprite = empty;
                if (iterations.gameObject.activeSelf)
                    iterations.gameObject.SetActive(false);
                return;
            }else{
                if (slot.GetSO() != oldSlotOccupants[index]){
                    if (currentSuperPassive != null && currentSuperPassive.GetDevelopState()){
                        currentSuperPassive.SetDevelopState(false);
                        currentSuperPassive = null;
                    }
                }
            }
            index++;
            EvaluateOperators(slot.GetSO().pip.GetCurrentPassive());
        }
        number = Mathf.Abs(Mathf.RoundToInt(number));

        //Debug.Log("Number got initially: "+number);

        if (number > SuperPassivePool.GetSuperPassiveCount()){
            float times = Mathf.Floor(number/SuperPassivePool.GetSuperPassiveCount());
            number -= SuperPassivePool.GetSuperPassiveCount()*times;
        }

        Debug.Log("Number got post-processing: "+number);

        currentSuperPassive = SuperPassivePool.GetPassiveByIndex(Mathf.RoundToInt(number));
        if (!currentSuperPassive.GetDevelopState()){
            currentSuperPassive.SetDevelopState(true);
        }
        Debug.Log("CSP: "+currentSuperPassive);
        cspImage.sprite = GetPassiveInfoByName(currentSuperPassive.gameObject.name).itemImage;
        iterations.text = "x"+currentSuperPassive.GetIterations();
        if (!iterations.gameObject.activeSelf)
            iterations.gameObject.SetActive(true);
        //Debug.Log("Current Super Passive:" + Mathf.RoundToInt(number));
        UpdateOldSlotOccupants();
    }

    private void UpdateOldSlotOccupants(){
        oldSlotOccupants.Clear();
        foreach (Slot slot in slots){
            oldSlotOccupants.Add(slot.GetSO());
        }
    }

    private void AssignPIC(){
        if (slots.Length == 4){
            PICVier = this;
        }else if (slots.Length == 3){
            PICTri = this;
        }else if (slots.Length == 2){
            PICDuo = this;
        }else{
            Debug.Log("Uhhhh slots length doesnt match up. Slot count: "+slots.Length);
        }  
    }

    void Awake(){
        UpdateOldSlotOccupants();
        AssignPIC();
        empty = cspImage.sprite;
        RegenerateSlots();
    }

    private void Update(){
        EvaluateStructure();
    }
}
