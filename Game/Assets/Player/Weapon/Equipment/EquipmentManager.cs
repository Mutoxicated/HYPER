using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager eq;
    private float maxPopulation = 4;
    [SerializeField] private List<PopulativeItem> equips = new List<PopulativeItem>();

    private void Awake(){
        if (eq != null && eq != this){
            Destroy(gameObject);
        }
        else {
            eq = this;
        }
    }

    public static void UpdateRunDataEquipment(){
        RunDataSave.rData.equipment.Clear();
        foreach (PopulativeItem pi in eq.equips){
            if (!pi.gameObject.activeSelf)
                continue;
            RunDataSave.rData.equipment.Add(new PopulativeInfo(pi.name,pi.GetPopulation()));
        }
    }

    private void RegenerateEquips(){
        if (RunDataSave.rData.equipment == null){
            return;
        }
        //Debug.Log("REGENERATING EQUIPS: ");
        foreach (PopulativeInfo equip in RunDataSave.rData.equipment){
            //Debug.Log(equip.name+" REGENERATED");
            AddEquipmentByName(equip.name,equip.population);
        }
    }

    private void Start(){
        foreach (PopulativeItem equip in equips){
            if (!equip.gameObject.activeInHierarchy)
                continue;
            equip.gameObject.SetActive(false);
            //equip.gameObject.SetActive(true);
        }
        RegenerateEquips();
    }

    public static void RevertAllEquipment(){
        foreach (PopulativeItem pi in eq.equips){
            if (!pi.gameObject.activeSelf)
                continue;
            pi.gameObject.SetActive(false);
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

    public bool AddEquipmentByName(string name, int population){
        PopulativeItem item = null;
        foreach (PopulativeItem it in equips){
            if (it.name == name)
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
        item.SetPopulation(Mathf.RoundToInt(Mathf.Clamp(population,0,maxPopulation)));
        return true; 
    }
}
