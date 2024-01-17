using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    private static int rerollCost = 3;
    private static int restoreCost = 20;

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

    private void GetRandomItems(int amount){
        modifiableItems = items.ToArray().ToList();
        currentItems.Clear();
        int rn = 0;
        for (int i = 0; i <  items.Count;i++){
            if (i > amount-1)
                break;
            rn = Random.Range(0,modifiableItems.Count);
            Debug.Log(rn);
            currentItems.Add(modifiableItems[rn]);
            modifiableItems.Remove(modifiableItems[rn]);
        }
        
        foreach (ItemSubscriber es in itemSubs){
            es.UpdateItem();
        }
    }

    private void SpendMoney(){
        if (PlayerInfo.GetMoney() < rerollCost)
            return;
        PlayerInfo.SetMoney(-rerollCost);
    }

    public void Reroll(){
        GetRandomItems(3);
        SpendMoney();
    }

    public void RestoreHealth(){
        if (PlayerInfo.GetMoney() < restoreCost)
            return;
        PlayerInfo.SetMoney(-restoreCost);
        PlayerInfo.GetGun().stats.numericals["health"] += 9999999f;
    }

    private void GetRandomGunEquipment(int amount){
        modifiableEquips = equipments.ToArray().ToList();
        currentEquips.Clear();
        int rn = 0;
        for (int i = 0; i < equipments.Count;i++){
            if (i > amount-1)
                break;
            rn = Random.Range(0,modifiableEquips.Count);
            Debug.Log(rn);
            currentEquips.Add(modifiableEquips[rn]);
            modifiableEquips.Remove(modifiableEquips[rn]);
        }
        
        foreach (EquipSubscriber es in equipSubs){
            es.UpdateEquipment();
        }
    }
}
