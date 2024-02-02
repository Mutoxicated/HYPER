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

    private string classesCombined;
    private ItemSubscriber currentIShovering;

    private void classStringBuilder(List<classType> classes){
        classesCombined = "";
        for (int i = 0; i < classes.Count; i++){
            if (i == 0){
                classesCombined += classes[i].ToString();
            }else{
                classesCombined += ", ";
                classesCombined += classes[i].ToString();
            }
        }
    }
    
    private void Update(){
        if (currentIShovering == null){
            foreach (ItemSubscriber i in its){
                if (!i.gameObject.activeSelf)
                    continue;
                if (i.hovering){
                    classStringBuilder(i.currentItem.item.GetClasses());
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
            }
        }
    }
}
