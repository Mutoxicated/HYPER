using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateShopUIElements : MonoBehaviour
{
    [SerializeField] private TMP_Text rerollCost;
    [SerializeField] private TMP_Text healthRestoreCost;
    [SerializeField] private TMP_Text dynamiteRestoreCost;
    [SerializeField] private TMP_Text dynamiteAddCost;

    private string ms = "*";//moneySymbol

    private void Start(){
        rerollCost.text = ItemShop.rerollCost.ToString()+ms;
        healthRestoreCost.text = ItemShop.restoreCost.ToString()+ms;
        dynamiteRestoreCost.text = ItemShop.dynamiteRestoreCost.ToString()+ms;
        dynamiteAddCost.text = ItemShop.dynamiteAddCost.ToString()+ms;
    }
}
