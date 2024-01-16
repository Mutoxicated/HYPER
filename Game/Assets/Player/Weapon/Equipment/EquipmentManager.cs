using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    private float maxPopulation = 1;
    [SerializeField] private List<PopulativeItem> equips = new List<PopulativeItem>();

    public bool AddEquipment(Equipment equipment){
        PopulativeItem item = null;
        foreach (PopulativeItem it in equips){
            if (it.name == equipment.name)
                item = it;
        }
        if (item == null){
            Debug.Log("No equipment with that name found.");
            return false;
        }
        if (!item.gameObject.activeSelf){
            item.gameObject.SetActive(true);
        }
        if (item.GetPopulation() < maxPopulation){
            item.AddPopulation(1);
            return true;
        }else{
            return false;
        }
    }
}
