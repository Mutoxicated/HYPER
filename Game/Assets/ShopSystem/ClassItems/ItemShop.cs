using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public static float cheapnessMod = 1f;

    public static int dynamiteRestoreCost = 15;
    public static int dynamiteAddCost = 7;
    public static int rerollCost = 3;
    public static int restoreCost = 20;

    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<Equipment> equipments = new List<Equipment>();

    [HideInInspector] public List<Item> currentItems = new List<Item>();
    [HideInInspector] public List<Equipment> currentEquips = new List<Equipment>();
    private List<Item> modifiableItems = new List<Item>();
    private List<Equipment> modifiableEquips = new List<Equipment>();

    [Header("Scene-related")]
    [SerializeField] private List<ItemSubscriber> itemSubs = new List<ItemSubscriber>();
    [SerializeField] private List<EquipSubscriber> equipSubs = new List<EquipSubscriber>();

    void Start()
    {
        GetRandomItems(3);
        GetRandomGunEquipment(2);
    }

    public static void ResetCheapness(){
        cheapnessMod = 1f;
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
        currentItems.Clear();
        int rn = 0;
        for (int i = 0; i <  items.Count;i++){
            if (i > amount-1)
                break;
            rn = SeedGenerator.random.Next(0,modifiableItems.Count);
            Debug.Log(rn);
            currentItems.Add(modifiableItems[rn]);
            modifiableItems.Remove(modifiableItems[rn]);
        }
        
        foreach (ItemSubscriber es in itemSubs){
            es.UpdateItem();
        }
    }

    public int Processed(float num){
        return Mathf.RoundToInt(num*cheapnessMod);
    }

    public void Reroll(){
        if (ValidateCost(rerollCost))
            GetRandomItems(3);
    }

    private void GetRandomGunEquipment(int amount){
        modifiableEquips = equipments.ToArray().ToList();
        currentEquips.Clear();
        int rn = 0;
        for (int i = 0; i < equipments.Count;i++){
            if (i > amount-1)
                break;
            rn = SeedGenerator.random.Next(0,modifiableEquips.Count);
            Debug.Log(rn);
            currentEquips.Add(modifiableEquips[rn]);
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
