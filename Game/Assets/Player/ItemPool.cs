using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    private static List<ClassItem> classItems = new List<ClassItem>();

    public static void ResetClassItems(){
        classItems.Clear();
    }

    public static List<ClassItem> GetClassItems(){
        return classItems;
    }

    public static List<ClassItem> GetClassItemsFromHierarchy(ClassHierarchy classHierarchy) {
        List<ClassItem> filteredItems = new List<ClassItem>();

        foreach (var item in classItems) {
            if (item.classHierarchy == classHierarchy) {
                filteredItems.Add(item);
            }
        }

        return filteredItems;
    }

    private void Awake(){
        if (classItems.Count == 0)
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
            classItems.Add(Instantiate(FindPrefabByName(name)).GetComponent<ClassItem>());
            
        }
    }

    public ClassItem FindClassItemFromItem(Item item){
        string goName = item.name.Replace("A","");
        foreach (ClassItem ci in classItems){
            if (ci.gameObject.name.Replace("(Clone)","") == goName){
                return ci;
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
        ClassItem ci = FindClassItemFromItem(item);
        if (ci != null){
            Debug.LogError("Item was bought but it already exists in item pool.");
            return false;
        }
        GameObject prefab = FindPrefabFromItem(item);
        if (prefab != null){
            GameObject instance = Instantiate(prefab);
            AddItemToData(item.name.Replace("A",""));
            classItems.Add(instance.GetComponent<ClassItem>());
        }else {
            Debug.LogError("Couldn't find prefab from item");
        }
        return true;
    }

    public void RemoveItem(Item item){
        ClassItem ci = FindClassItemFromItem(item);
        if (ci != null){
            classItems.Remove(ci);
            RemoveItemFromData(item.name.Replace("A",""));
            Destroy(ci.gameObject);
        }
    }
}
