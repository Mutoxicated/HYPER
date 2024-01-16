using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    private static int rerollCost = 3;
    private static int restoreCost = 20;

    [SerializeField] private EquipmentManager em;

    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<Equipment> equipments = new List<Equipment>();

    [HideInInspector] public List<Item> currentItems = new List<Item>();
    [HideInInspector] public List<Item> currentEquips = new List<Item>();
    private List<Item> modifiableItems = new List<Item>();
    private List<Item> modifiableEquips = new List<Item>();

    public delegate void Sub();
    public List<Sub> subscribers = new List<Sub>();

    void Start()
    {
        GetRandomGunEquipment(2);
        NewItems();
    }

    private void GetRandomItems(int amount){
        modifiableItems = items.ToArray().ToList();
        currentItems.Clear();
        int rn = 0;
        for (int i = 0; i < amount;i++){
            rn = Random.Range(0,modifiableItems.Count);
            Debug.Log(rn);
            currentItems.Add(modifiableItems[rn]);
            modifiableItems.Remove(modifiableItems[rn]);
        }
        Debug.Log("items: "+currentItems.Count);
    }

    private void NewItems(){
        GetRandomItems(3);
        foreach (Sub sub in subscribers){
            sub.Invoke();
        }
    }

    private void SpendMoney(){
        if (PlayerInfo.GetMoney() < rerollCost)
            return;
        PlayerInfo.SetMoney(-rerollCost);
    }

    public void Reroll(){
        NewItems();
        SpendMoney();
    }

    public void RestoreHealth(){
        if (PlayerInfo.GetMoney() < restoreCost)
            return;
        PlayerInfo.SetMoney(-restoreCost);
        PlayerInfo.GetGun().stats.numericals["health"] += 9999999f;
    }

    private void GetRandomGunEquipment(int amount){
        modifiableEquips = items.ToArray().ToList();
        currentEquips.Clear();
        int rn = 0;
        for (int i = 0; i < amount;i++){
            rn = Random.Range(0,modifiableEquips.Count);
            Debug.Log(rn);
            currentEquips.Add(modifiableEquips[rn]);
            modifiableEquips.Remove(modifiableEquips[rn]);
        }
        Debug.Log("items: "+currentEquips.Count);
    }

    public void BuyEquipment(Equipment equip){

    }
}
