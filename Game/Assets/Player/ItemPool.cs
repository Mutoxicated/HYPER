using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    private static List<GameObject> classItems = new List<GameObject>();

    private void Awake(){
        classItems.Clear();
        RegenerateItems();
        PlayerInfo.SetIP(this);
    }

    public static void AddItemToData(string itemName){
        if (!RunDataSave.rData.activeClassItems.Contains(itemName))
            RunDataSave.rData.activeClassItems.Add(itemName);
    }

    public static void RemoveItemFromData(string itemName){
        if (RunDataSave.rData.activeClassItems.Contains(itemName))
            RunDataSave.rData.activeClassItems.Remove(itemName);
    }

    private void RegenerateItems(){
        if (RunDataSave.rData.activeClassItems.Count == 0) return;

        foreach (string name in RunDataSave.rData.activeClassItems){
            classItems.Add(Instantiate(FindPrefabByName(name)));
        }
    }

    private GameObject FindGoFromItem(Item item){
        string goName = item.name.Replace("A","");
        foreach (GameObject ci in classItems){
            if (ci.name.Replace("(Clone)","") == goName){
                return ci.gameObject;
            }
        }
        return null;
    }

    private GameObject FindPrefabFromItem(Item item){
        string goName = item.name.Replace("A","");
        foreach (GameObject g in prefabs){
            if (g.name == goName){
                return g;
            }
        }
        return null;
    }

    private GameObject FindPrefabByName(string name){
        foreach (GameObject g in prefabs){
            if (g.name == name){
                return g;
            }
        }
        return null;
    }

    public bool AddItem(Item item){
        Debug.Log("Added an item");
        GameObject go = FindGoFromItem(item);
        if (go != null)
            return false;
        GameObject prefab = FindPrefabFromItem(item);
        if (prefab != null){
            GameObject instance = Instantiate(prefab);
            AddItemToData(item.name.Replace("A",""));
            classItems.Add(instance);
        }
        return true;
    }

    public void RemoveItem(Item item){
        GameObject go = FindGoFromItem(item);
        if (go != null){
            classItems.Remove(go);
            RemoveItemFromData(item.name.Replace("A",""));
            Destroy(go);
        }
    }
}
