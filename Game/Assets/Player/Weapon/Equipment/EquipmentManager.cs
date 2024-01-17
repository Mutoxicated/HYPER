using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager eq;
    private float maxPopulation = 4;
    [SerializeField] private List<PopulativeItem> equips = new List<PopulativeItem>();

    private void Awake(){
        if (eq != null && eq != this) {
            Destroy(gameObject);
        }
        else {
            eq = this;
        }
    }

    private void Start(){
        foreach (PopulativeItem equip in equips){
            if (!equip.gameObject.activeInHierarchy)
                continue;
            equip.gameObject.SetActive(false);
            //equip.gameObject.SetActive(true);
        }
    }

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
            return true;
        }
        if (item.GetPopulation() < maxPopulation){
            item.AddPopulation(1);
            return true;
        }
        return false; 
    }
}
