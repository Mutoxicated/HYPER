using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text nameInfo;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Transform[] classPlacements;

    private List<classType> classes = new List<classType>();
    private Item cachedItem;

    private void ClassUpdate(ItemSubscriber iss){
        for (int i = 0; i < classes.Count; i++){
            ClassSystem.classDict[classes[i]].gameObject.SetActive(true);
            ClassSystem.classDict[classes[i]].transform.SetParent(classPlacements[i].transform,true);
            ClassSystem.classDict[classes[i]].transform.localPosition = Vector3.zero;
            if (iss.IsBeingSold())
                ClassSystem.classDict[classes[i]].PendBatteryDecrease();
            else
                ClassSystem.classDict[classes[i]].PendBattery();
        }
    }

    private void UnpendClasses(){
        for (int i = 0; i < classes.Count; i++){
            ClassSystem.classDict[classes[i]].gameObject.SetActive(false);
            ClassSystem.classDict[classes[i]].GoBackToParent();
            ClassSystem.classDict[classes[i]].UnpendBattery();
        }
    }

    public Item GetCurrentCachedItem(){
        return cachedItem;
    }
    
    public void UpdateInfo(ItemSubscriber iss, Item item){
        if (cachedItem == null){
            cachedItem = item;
        }

        if (iss == null){
            if (cachedItem != item)
                return;
            cachedItem = null;
            nameInfo.text = "";
            description.text = "Description: ";
            UnpendClasses();
        }else{
            classes = iss.currentItem.item.GetClasses();
            ClassUpdate(iss);
            
            nameInfo.text = iss.currentItem.itemName;
            nameInfo.color = iss.currentItem.nameColor;
            description.text = "Description: "+ iss.currentItem.description;
            if (iss.IsBeingSold()){
                description.text += " Selling at "+iss.currentItem.cost*ItemShop.sellMultiplier+".";
            }
        }
    }
    
}
