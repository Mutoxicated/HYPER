using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Battery {
    public int cellAmount = 4;
    private float currentCells = 0;
    private int flooredCells = 0;

    public void Increase(float num){
        currentCells += num;
        if (currentCells > cellAmount){
            currentCells = cellAmount;
        }
        if (currentCells < 0){
            currentCells = 0;
        }
        flooredCells = Mathf.FloorToInt(currentCells);
    }

    public int GetCurrentCells(){
        return flooredCells;
    }

    public float GetUnflooredCells(){
        return currentCells;
    }

    public Battery(int cellAmount){
        this.cellAmount = cellAmount;
    }
}

public class Class : MonoBehaviour
{
    public static readonly int[] maxItemsOfClass = new int[3]{
        4,8,14
    };
    public static readonly int[] batteryAmountOfClass = new int[3]{
        1,2,2
    };
    public static readonly int[] cellAmountOfClass = new int[3]{
        4,4,6
    };

    [SerializeField] private ClassInfo identity;
    [SerializeField] private List<BatteryDresser> bds = new List<BatteryDresser>();
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text className;

    private List<Battery> batteries = new List<Battery>();
    private int totalCells;
    private Transform parent;

    private void Awake(){
        int index = (int)identity.hierarchy;
        for (int i = 0; i < batteryAmountOfClass[index]; i++){
            totalCells += cellAmountOfClass[index];
            batteries.Add(new Battery(cellAmountOfClass[index]));
        }
        image.sprite = identity.image;
        parent = transform.parent.transform;
        className.text = identity.name;
        className.color = identity.color;
    }

    public ClassInfo PapersPlease(){
        return identity;
    }

    public void GoBackToParent(){
        transform.SetParent(parent,true);
        transform.localPosition = Vector3.zero;
    }

    public int IncreaseBattery(){
        for (int i = 0; i < batteries.Count; i++){
            if (batteries[i].GetCurrentCells() == batteries[i].cellAmount){
                continue;
            }
            batteries[i].Increase(maxItemsOfClass[(int)identity.hierarchy]/totalCells);
            bds[i].Increase(batteries[i].GetCurrentCells());
            return (batteries[i].GetCurrentCells() == batteries[i].cellAmount) ? batteries[i].cellAmount : -1;
        }
        return -1;
    }

    public int DecreaseBattery(){
        int cellAmount = -1;
        for (int i = batteries.Count-1; i >= 0; i--){
            if (batteries[i].GetCurrentCells() == 0){
                continue;
            }
            if (batteries[i].GetCurrentCells() == batteries[i].cellAmount){
                cellAmount = batteries[i].cellAmount;
            }
            batteries[i].Increase(-maxItemsOfClass[(int)identity.hierarchy]/totalCells);
            bds[i].Increase(batteries[i].GetCurrentCells());
        }
        return cellAmount;
    }

    public void PendBattery(){
        for (int i = 0; i < batteries.Count; i++){
            if (batteries[i].GetCurrentCells() == batteries[i].cellAmount){
                continue;
            }
            if (bds[i].GetPending()) continue;
            float hypotheticalIncrease = batteries[i].GetUnflooredCells()+maxItemsOfClass[(int)identity.hierarchy]/totalCells;
            bds[i].SetPending(batteries[i].GetCurrentCells(),Mathf.FloorToInt(hypotheticalIncrease),false);
        }
    }

    public void PendBatteryDecrease(){
        for (int i = 0; i < batteries.Count; i++){
            if (batteries[i].GetCurrentCells() == batteries[i].cellAmount){
                continue;
            }
            if (bds[i].GetPending()) continue;
            float hypotheticalDecrease = batteries[i].GetUnflooredCells()-maxItemsOfClass[(int)identity.hierarchy]/totalCells;
            bds[i].SetPending(Mathf.FloorToInt(hypotheticalDecrease),batteries[i].GetCurrentCells(),true);
        }
    }

    public void UnpendBattery(){
        for (int i = 0; i < bds.Count; i++){
            if (bds[i].GetPending())
                bds[i].DeactivatePending();
        }
    }
}
