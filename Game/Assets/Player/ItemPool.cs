using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    private static ItemPool IP;
    [SerializeField] private List<ClassItem> classObjects = new List<ClassItem>();

    [HideInInspector] public static List<ClassItem> enabledItems = new List<ClassItem>();

    public static void ResetClassItems(){
        enabledItems.Clear();
    }

    public static List<ClassItem> GetClassItems(){
        return PlayerInfo.GetIP().classObjects;
    }

    public static bool Contains(Item item) {
        foreach (var ci in enabledItems) {
            if (ci.itemInfo.itemName == item.itemName) {
                return true;
            }
        }
        return false;
    }

    public void AddItemsToClasses() {
        foreach (var item in classObjects) {
            //Debug.Log("Item: "+item.itemInfo.itemName);
            foreach (var _class in item.Classes) {
                //Debug.Log(" Class: "+_class);
                ClassSystem.classDict[_class].classItems.Add(item);
            }
        }
    }

    public static List<ClassItem> GetClassItemsFromHierarchy(ClassHierarchy classHierarchy) {
        List<ClassItem> filteredItems = new List<ClassItem>();

        foreach (var item in PlayerInfo.GetIP().classObjects) {
            if (item.classHierarchy == classHierarchy) {
                filteredItems.Add(item);
            }
        }

        return filteredItems;
    }

    private void Awake(){
        if (IP != null && IP != this){
            Destroy(gameObject);
        }
        else {
            if (enabledItems.Count == 0)
                RegenerateItems();
            IP = this;
            PlayerInfo.SetIP(IP);
        }
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
            ClassItem ci = FindObjectByName(name);
            ci.Enable();
            enabledItems.Add(ci);
        }
    }

    public ClassItem FindFromItem(Item item, List<ClassItem> list){
        string goName = item.name.Replace("A","");
        foreach (ClassItem ci in list){
            if (ci.gameObject.name == goName){
                return ci;
            }
        }
        return null;
    }

    private ClassItem FindObjectByName(string name){
        foreach (ClassItem g in classObjects){
            if (g.gameObject.name == name){
                return g;
            }
        }
        return null;
    }

    public bool AddItem(Item item){
        ClassItem ci = FindFromItem(item, enabledItems);
        if (ci != null){
            return false;
        }
        ClassItem go = FindFromItem(item, classObjects);
        if (go != null){
            Debug.Log("Enabled item "+item.itemName);
            AddItemToData(item.name.Replace("A",""));
            go.Enable();
            enabledItems.Add(go.GetComponent<ClassItem>());
        }else {
            Debug.LogError("Couldn't find class object from item");
        }
        return true;
    }

    public void RemoveItem(Item item){
        ClassItem ci = FindFromItem(item, enabledItems);
        if (ci != null){
            enabledItems.Remove(ci);
            RemoveItemFromData(item.name.Replace("A",""));
            ci.Disable();
        }
    }
}
