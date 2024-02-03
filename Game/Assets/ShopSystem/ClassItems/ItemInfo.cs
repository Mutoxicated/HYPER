using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private List<ItemSubscriber> its = new List<ItemSubscriber>();

    [SerializeField] private TMP_Text classInfo;
    [SerializeField] private TMP_Text description;
    [SerializeField] private Transform[] classPlacements;

    private string classesCombined;
    private ItemSubscriber currentIShovering;
    private List<classType> classes = new List<classType>();

    private void classStringBuilder(){
        classesCombined = "";
        for (int i = 0; i < classes.Count; i++){
            if (i == 0){
                classesCombined += classes[i].ToString();
            }else{
                classesCombined += ", ";
                classesCombined += classes[i].ToString();
            }
            ClassSystem.classDict[classes[i]].gameObject.SetActive(true);
            ClassSystem.classDict[classes[i]].transform.SetParent(classPlacements[i].transform,true);
            ClassSystem.classDict[classes[i]].transform.localPosition = Vector3.zero;
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
    
    private void Update(){
        if (currentIShovering == null){
            foreach (ItemSubscriber i in its){
                if (!i.gameObject.activeSelf)
                    continue;
                if (i.hovering){
                    classes = i.currentItem.item.GetClasses();
                    classStringBuilder();
                    classInfo.text = "Contained in: "+classesCombined;
                    description.text = i.currentItem.description;
                    currentIShovering = i;
                    return;
                }
            }
            classInfo.text = "";
            description.text = "";
        }else{
            if (!currentIShovering.hovering){
                currentIShovering = null;
                UnpendClasses();
            }
        }
    }
}
