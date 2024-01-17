using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSubscriber : MonoBehaviour
{
    [SerializeField] private ItemShop shop;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text eqName;
    [SerializeField] private int index;

    public void UpdateEquipment(){
        //Debug.Log("aaaaaaaa");
        image.sprite = shop.currentEquips[index].symbol;
        eqName.text = shop.currentEquips[index].name;
        eqName.text = eqName.text.Replace("_",":");
    }

    public void EquipTaken(){
        if (PlayerInfo.GetMoney() < shop.currentEquips[index].cost)
            return;
        bool success = EquipmentManager.eq.AddEquipment(shop.currentEquips[index]);
        if (!success)
            return;
        PlayerInfo.SetMoney(-shop.currentEquips[index].cost);
        gameObject.SetActive(false);
    }
}
