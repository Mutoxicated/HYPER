using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemsToSorter : MonoBehaviour
{
    [SerializeField] private Transform canvas;
    [SerializeField] private ActualItemSorter ais;
    [SerializeField] private ClassHierarchy classHierarchy;

    private List<ClassItem> clasItems = new List<ClassItem>();

    private void OnEnable() {
        clasItems = ItemPool.GetClassItemsFromHierarchy(classHierarchy);
        foreach (var item in clasItems) {
            item.aits = this;
            item.SellMode(true);
            item.EnableUIMode(canvas);
            ais.AddItem(item.transform);
        }
        ais.PrepareValues();
        ais.SortItems();
    }

    public void RemoveItem(ClassItem item) {
        ais.RevertItemScaling(item.transform);
        ais.RemoveItem(item.transform);
        item.DisableUIMode();
        clasItems.Remove(item);
        PlayerInfo.GetIP().RemoveItem(item.itemInfo);
        ais.SortItems();
    }

    private void DisableItems() {
        clasItems = ItemPool.GetClassItemsFromHierarchy(classHierarchy);
        ais.RevertItemScaling();
        foreach (var item in clasItems) {
            item.DisableUIMode();
        }
        ais.ClearItems();
    }

    private void OnDisable(){
        Invoke("DisableItems", 0.01f);
    }
}
