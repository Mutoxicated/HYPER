using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipSubscriber : MonoBehaviour
{
    [SerializeField] private bool beingSold;
    [SerializeField] private Equipment equip;
    [SerializeField] private ItemShop shop;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text eqName;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text population;
    [SerializeField] private int index;

    private PopulativeItem pi;

    private void Start(){
        DressEquip();
    }

    private void DressEquip(){
        if (!beingSold){
            return;
        }
        image.sprite = equip.symbol;
        eqName.text = equip.name;
        eqName.text = eqName.text.Replace("_",":");
        cost.text = "Selling for: "+Mathf.RoundToInt(equip.cost*ItemShop.sellMultiplier)+"*";
    }

    public void UpdateEquipment(){
        //Debug.Log("aaaaaaaa");
        if (beingSold){
            return;
        }
        image.sprite = shop.currentEquips[index].symbol;
        eqName.text = shop.currentEquips[index].name;
        eqName.text = eqName.text.Replace("_",":");
        cost.text = shop.Processed(shop.currentEquips[index].cost).ToString()+"*";
    }

    private void Update(){
        if (!beingSold) return;
        pi = EquipmentManager.eq.GetPIByEquipment(equip);
        if (!pi.gameObject.activeSelf){
            population.text = "x0";
            return;
        }
        population.text = "x"+pi.GetPopulation();
    }

    public void EquipTaken(){
        if (PlayerInfo.GetMoney() < shop.Processed(shop.currentEquips[index].cost))
            return;
        bool success = EquipmentManager.eq.AddEquipment(shop.currentEquips[index]);
        if (!success)
            return;
        PlayerInfo.SetMoney(-shop.currentEquips[index].cost);
        gameObject.SetActive(false);
    }

    public void EquipRetrieved(){
        if (!pi.gameObject.activeSelf) return;
        PlayerInfo.SetMoney(Mathf.RoundToInt(equip.cost*ItemShop.sellMultiplier));
        EquipmentManager.eq.RemoveEquip(equip);
    }
}
