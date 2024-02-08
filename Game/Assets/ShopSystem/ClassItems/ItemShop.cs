using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class DisabableItem{
    public Item item;
    public Equipment equip;
    public bool activeSelf = true;

    public void SetActive(bool state){
        activeSelf = state;
    }

    public bool GetActive(){
        return activeSelf;
    }

    public DisabableItem(Item item){
        this.item = item;
    }

    public DisabableItem(Equipment equip){
        this.equip = equip;
    }
}

public class ItemShop : MonoBehaviour
{
    public static float sellMultiplier = 0.5f;
    public static float cheapnessMod = 1f;
    public static float expensiveMod = 1f;

    public static int dynamiteRestoreCost = 15;
    public static int dynamiteAddCost = 7;
    public static int rerollCost = 3;
    public static int restoreCost = 20;

    private static bool getRunData = true;
    public static ItemShop IS;

    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<Equipment> equipments = new List<Equipment>();

    [HideInInspector] public List<DisabableItem> currentItems = new List<DisabableItem>();
    [HideInInspector] public List<DisabableItem> currentEquips = new List<DisabableItem>();
    private List<Item> modifiableItems = new List<Item>();
    private List<Equipment> modifiableEquips = new List<Equipment>();

    [Header("Scene-related")]
    [SerializeField] private List<ItemSubscriber> itemSubs = new List<ItemSubscriber>();
    [SerializeField] private List<EquipSubscriber> equipSubs = new List<EquipSubscriber>();

    void Start()
    {
        IS = this;
        currentItems.Clear();
        currentEquips.Clear();
        Debug.Log("get run data? "+getRunData);
        if (RunDataSave.rData.currentShopEquips.Count == 0){
            Randomize();
        }else{
            if (!getRunData){
                Randomize();
                return;
            }
            RegenerateItems();
        }
    }

    private void RegenerateItems(){
        currentItems = RunDataSave.rData.currentShopItems;
        foreach (ItemSubscriber es in itemSubs){
            es.UpdateItem();
        }
        currentEquips = RunDataSave.rData.currentShopEquips;
        foreach (EquipSubscriber es in equipSubs){
            es.UpdateEquipment();
        }
        getRunData = false;
    }

    public static void UpdateCurrentItems(){
        RunDataSave.rData.currentShopItems = IS.currentItems;
        RunDataSave.rData.currentShopEquips = IS.currentEquips;
    }

    private void Randomize(){
        GetRandomItems(3);
        GetRandomGunEquipment(2);
        getRunData = false;
    }

    public static void ResetCheapness(){
        cheapnessMod = 1f;
        expensiveMod = 1f;
        sellMultiplier = 1f;
        getRunData = true;
    }

    private bool ValidateCost(int cost){
        if (PlayerInfo.GetMoney() < Processed(cost)){
            return false;
        }
        PlayerInfo.SetMoney(-Processed(cost));
        return true;
    }

    private void GetRandomItems(int amount){
        modifiableItems = items.ToArray().ToList();
        int rn = 0;
        for (int i = 0; i <  items.Count;i++){
            if (i > amount-1)
                break;
            rn = SeedGenerator.random.Next(0,modifiableItems.Count);
            Debug.Log(rn);
            currentItems.Add(new DisabableItem(modifiableItems[rn]));
            modifiableItems.Remove(modifiableItems[rn]);
        }
        
        foreach (ItemSubscriber es in itemSubs){
            es.UpdateItem();
        }
    }

    public int Processed(float num){
        return Mathf.RoundToInt(num*cheapnessMod*expensiveMod);
    }

    public void Reroll(){
        if (ValidateCost(rerollCost))
            GetRandomItems(3);
    }

    private void GetRandomGunEquipment(int amount){
        modifiableEquips = equipments.ToArray().ToList();
        int rn = 0;
        for (int i = 0; i < equipments.Count;i++){
            if (i > amount-1)
                break;
            rn = SeedGenerator.random.Next(0,modifiableEquips.Count);
            Debug.Log(rn);
            currentEquips.Add(new DisabableItem(modifiableEquips[rn]));
            modifiableEquips.Remove(modifiableEquips[rn]);
        }
        
        foreach (EquipSubscriber es in equipSubs){
            es.UpdateEquipment();
        }
    }

    public void RestoreHealth(){
        if (ValidateCost(restoreCost)){
            PlayerInfo.GetGun().stats.numericals["health"] += 9999999f;
        }
    }

    public void RestoreDynamite(){
        if (PlayerInfo.GetGun().stats.numericals["capacitor1"] < PlayerInfo.GetGun().stats.numericals["maxCapacitor1"]){
            if (ValidateCost(dynamiteRestoreCost)){
                PlayerInfo.GetGun().stats.numericals["capacitor1"] = PlayerInfo.GetGun().stats.numericals["maxCapacitor1"];
            }
        }
    }

    public void AddDynamite(){
        if (PlayerInfo.GetGun().stats.numericals["capacitor1"] < PlayerInfo.GetGun().stats.numericals["maxCapacitor1"]){
            if (ValidateCost(dynamiteAddCost)){
                PlayerInfo.GetGun().stats.numericals["capacitor1"]++;
            }
        }else{
            if (ValidateCost(dynamiteAddCost)){
                PlayerInfo.GetGun().stats.numericals["capacitor1"]++;
                PlayerInfo.GetGun().stats.numericals["maxCapacitor1"]++;
            }
        }
    }
}
