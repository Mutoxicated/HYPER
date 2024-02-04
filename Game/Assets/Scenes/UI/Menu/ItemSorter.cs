using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSorter : MonoBehaviour
{
    [SerializeField] private Transform[] placements;
    [SerializeField] private ItemSubscriber[] items;
    [SerializeField] private List<ItemSubscriber> currentExistingItems;

    public void Structure(){
        currentExistingItems.Clear();
        foreach (ItemSubscriber item in items){
            if (item.DetectExistenceInShop()){
                currentExistingItems.Add(item);
            }
        }
        if (currentExistingItems.Count == 0)
            return;
        for (int i = 0; i < currentExistingItems.Count; i++){
            currentExistingItems[i].transform.position = placements[i].position;
        }
    }

    public void OnEnable(){
        Structure();
    }
}
