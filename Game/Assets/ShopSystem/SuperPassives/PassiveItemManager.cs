using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Linq;

public class PassiveItemManager : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private List<PassiveItemInfo> passiveItemInfos = new List<PassiveItemInfo>();
    [SerializeField] private List<PassiveItemPresenter> pips = new List<PassiveItemPresenter>();

    private List<PassiveItemInfo> TEMPpassiveItemInfos;
    private static bool getRandom = false;

    public Slot[] GetSlots(){
        return slots;
    }

    public List<PassiveItemInfo> GetPassiveInfos(){
        return passiveItemInfos;
    }

    public PassiveItemInfo GetPassiveItemInfoByPassiveName(string name){
        //Debug.Log("Getting PII by Passive Name, name given: "+ name);
        foreach (PassiveItemInfo pii in passiveItemInfos){
            if (pii.itemName.ToUpper().Replace(" ","_") == name){
                return pii;
            }
        }
        return null;
    }

    public void UpdateFilledSlots(string sceneName){
        SetRandomState(true);
        if (sceneName == "Interoid") return;
        if (RunDataSave.rData.passiveSlotsFilled.Count == 5)
            RunDataSave.rData.passiveSlotsFilled = new List<PassiveItemInfo>(){null,null,null,null};
        for (int i = 0; i < slots.Length; i++){
            //Debug.Log("Passive slot: "+slots[i].GetSO());
            if (slots[i].GetSO() == null)
                RunDataSave.rData.passiveSlotsFilled[i] = null;
            else
                RunDataSave.rData.passiveSlotsFilled[i] = GetPassiveItemInfoByPassiveName(slots[i].GetSO().pip.GetCurrentPassive().gameObject.name);
        }
    }

    public List<PassiveItemInfo> DecideListToUse(){
        if (getRandom)
            return null;

        if (RunDataSave.rData.passiveSlotsFilled.Count == 5)
            return null;
        return RunDataSave.rData.passiveSlotsFilled;
    }

    public void SetRandomState(bool state){
        getRandom = state;
    }

    private void PresentPassives(){
        TEMPpassiveItemInfos = passiveItemInfos.ToList();
        foreach (PICSlot ps in PIC.PICVier.GetCurrentPICSlots()){
            TEMPpassiveItemInfos.Remove(ps.pii);
        }
        foreach (PICSlot ps in PIC.PICTri.GetCurrentPICSlots()){
            TEMPpassiveItemInfos.Remove(ps.pii);
        }
        foreach (PICSlot ps in PIC.PICDuo.GetCurrentPICSlots()){
            TEMPpassiveItemInfos.Remove(ps.pii);
        }
        List<PassiveItemInfo> decidedList = DecideListToUse();
        for (int i = 0; i < pips.Count; i++){
            if (TEMPpassiveItemInfos.Count < i){
                Destroy(slots[i].GetSO().gameObject);
                slots[i].SetSO(null);
                continue;
            }
            int num = SeedGenerator.random.Next(0,TEMPpassiveItemInfos.Count);
            if (decidedList != null){
                if (decidedList[i] == null){
                    Destroy(slots[i].GetSO().gameObject);
                    slots[i].SetSO(null);
                    continue;
                }else{
                    pips[i].SetImageSprite(decidedList[i].itemImage);
                    pips[i].SetTitle(decidedList[i].itemName,decidedList[i].nameColor);
                    pips[i].SetCurrentPassive(decidedList[i].item);
                    TEMPpassiveItemInfos.Remove(decidedList[i]);
                    continue;
                }
            }

            pips[i].SetImageSprite(TEMPpassiveItemInfos[num].itemImage);
            pips[i].SetTitle(TEMPpassiveItemInfos[num].itemName,TEMPpassiveItemInfos[num].nameColor);
            pips[i].SetCurrentPassive(TEMPpassiveItemInfos[num].item);
            TEMPpassiveItemInfos.Remove(TEMPpassiveItemInfos[num]);
        }
    }

    private void Start(){
        PresentPassives();
    }
}
