using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private List<Battery> batteries = new List<Battery>();
    private int totalCells;

    private void Awake(){
        int index = (int)identity.hierarchy;
        for (int i = 0; i < batteryAmountOfClass[index]; i++){
            batteries.Add(new Battery(cellAmountOfClass[index]));
        }
    }

    public ClassInfo PapersPlease(){
        return identity;
    }

    public int IncreaseBattery(){
        for (int i = 0; i < batteries.Count; i++){
            if (batteries[i].GetCurrentCells() == batteries[i].cellAmount){
                continue;
            }
            batteries[i].Increase(maxItemsOfClass[(int)identity.hierarchy]/totalCells);
            return (batteries[i].GetCurrentCells() == batteries[i].cellAmount) ? batteries[i].cellAmount : -1;
        }
        return -1;
    }

    public int DecreaseBattery(){
        int cellAmount = -1;
        for (int i = batteries.Count-1; i >= 0; i++){
            if (batteries[i].GetCurrentCells() == 0){
                continue;
            }
            if (batteries[i].GetCurrentCells() == batteries[i].cellAmount){
                cellAmount = batteries[i].cellAmount;
            }
            batteries[i].Increase(-maxItemsOfClass[(int)identity.hierarchy]/totalCells);
        }
        return cellAmount;
    }
}
