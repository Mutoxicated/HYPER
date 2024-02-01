using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class PICSlot{
    public PassiveItemInfo pii;
    public bool locked = false;

    public PICSlot(PassiveItemInfo pii, bool locked){
        this.pii = pii;
        this.locked = locked;
    }
}

public class PIC : MonoBehaviour
{
    public static PIC PICVier;
    public static PIC PICTri;
    public static PIC PICDuo;

    [SerializeField] private PassiveItemManager pim;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Image cspImage;
    [SerializeField] private TMP_Text iterations;
    [SerializeField] private Slot[] slots; 
    
    [SerializeField] private PassiveItemInfo[] sps;

    private float number;
    private SuperPassive currentSuperPassive;
    private Sprite empty;

    private List<PICSlot> currentPiis = new List<PICSlot>();
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

    public static void LockAllCurrentSlots(string sceneName){
        if (sceneName != "Interoid") return;
        PICVier.LockSlots();
        PICTri.LockSlots();
        PICDuo.LockSlots();
    }

    public Slot[] GetSlots(){
        return slots;
    }

    public PassiveItemManager GetPIM(){
        return pim;
    }

    public void SetCurrentPiis(){
        for (int i = 0; i < slots.Length; i++){
            Debug.Log("SETTING PIIS THROUGH SLOTS");
            //Debug.Log(slots[i].GetSO()+ " contains: "+);
            if (slots[i].GetSO() == null) {
                currentPiis[i].pii = null;
                continue;
            }
            Debug.Log("PASSIVE NAME: "+pim.GetPassiveItemInfoByPassiveName(slots[i].GetSO().pip.GetCurrentPassive().gameObject.name).name);
            currentPiis[i].pii = pim.GetPassiveItemInfoByPassiveName(slots[i].GetSO().pip.GetCurrentPassive().gameObject.name);
        }
    }

    public List<PICSlot> GetCurrentPICSlots(){
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

    public void LockSlots(){
        foreach (PICSlot ps in currentPiis){
            if (ps.pii != null){
                ps.locked = true;
            }
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
            if (currentPiis[i].pii == null){
                Destroy(slots[i].GetSO().gameObject);
                slots[i].SetSO(null);
                continue;
            }else{
                if (currentPiis[i].locked)
                    slots[i].GetSO().SetIgnoredSlots(GetPIM().GetSlots());
            }
            slots[i].GetSO().pip.SetImageSprite(currentPiis[i].pii.itemImage);
            slots[i].GetSO().pip.SetTitle(currentPiis[i].pii.itemName,currentPiis[i].pii.nameColor);
            slots[i].GetSO().pip.SetCurrentPassive(currentPiis[i].pii.item);
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
                itemName.text = "";
                description.text = "";
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

        if (number > SuperPassivePool.GetSuperPassiveCount()-1){
            float times = Mathf.Floor(number/(SuperPassivePool.GetSuperPassiveCount()-1));
            number -= (SuperPassivePool.GetSuperPassiveCount()-1)*times;
        }

        //Debug.Log("Number got post-processing: "+number);

        currentSuperPassive = SuperPassivePool.GetPassiveByIndex(Mathf.RoundToInt(number));
        Debug.Log(currentSuperPassive);
        if (currentSuperPassive == null) return;
        if (!currentSuperPassive.GetDevelopState()){
            currentSuperPassive.SetDevelopState(true);
        }
        //Debug.Log("CSP: "+currentSuperPassive);
        PassiveItemInfo pii = GetPassiveInfoByName(currentSuperPassive.gameObject.name);
        cspImage.sprite = pii.itemImage;
        iterations.text = "x"+currentSuperPassive.GetIterations();
        itemName.text = pii.itemName;
        itemName.color = pii.nameColor;
        description.text = pii.description;
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
