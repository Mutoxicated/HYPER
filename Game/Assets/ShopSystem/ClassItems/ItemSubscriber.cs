using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSubscriber : MonoBehaviour
{
    [SerializeField] private bool beingSold;
    [SerializeField] private ItemSorter iss;
    [SerializeField] private ItemInfo ii;
    [SerializeField] private ItemShop shop;
    [SerializeField] private int index;
    public Item currentItem;
    [Space]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private Image image;
    [SerializeField] private GameObject scalableObj;

    private Vector3 initScale;
    private Vector3 maxScale;
    private Vector3 currScale;

    [HideInInspector] public bool hovering = false;
    private float speed = 5f;

    public void Awake(){
        initScale = scalableObj.transform.localScale;
        maxScale = initScale*1.1f;
        currScale = initScale;
        if (beingSold)
            DressItem();
    }

    private void DressItem(){
        image.sprite = currentItem.itemImage;
    }

    public void UpdateItem(){
        if (!gameObject.activeSelf && !beingSold)
            gameObject.SetActive(true);
        currentItem = shop.currentItems[index].item;
        if (!beingSold){
            gameObject.SetActive(shop.currentItems[index].GetActive());
            title.text = currentItem.itemName;
            title.color = currentItem.nameColor;
            cost.text = shop.Processed(currentItem.cost).ToString()+"*";
        }
        image.sprite = currentItem.itemImage;
    }

    private void Update(){
        if (hovering){
            currScale = Vector3.Lerp(currScale,maxScale,Time.deltaTime*speed);
        }else{
            currScale = Vector3.Lerp(currScale,initScale,Time.deltaTime*speed);
        }
        scalableObj.transform.localScale = currScale;
    }

    public void IsHovering(bool state){
        hovering = state;
        if (state){
            ii.UpdateInfo(this,currentItem);
        }else{
            if (ii.GetCurrentCachedItem() == currentItem)
                ii.UpdateInfo(null,currentItem);
        }
    }

    public void ItemTaken(){
        if (PlayerInfo.GetMoney() < shop.Processed(currentItem.cost))
            return;
        bool success = PlayerInfo.GetIP().AddItem(currentItem);
        if (!success)
            return;
        PlayerInfo.SetMoney(-shop.Processed(currentItem.cost));
        gameObject.SetActive(false);
        shop.currentItems[index].SetActive(false);
        currScale = initScale;
        scalableObj.transform.localScale = currScale;
        hovering = false;
        ii.UpdateInfo(null,currentItem);
    }

    public void ItemRetrieved(){
        PlayerInfo.GetIP().RemoveItem(currentItem);

        PlayerInfo.SetMoney(shop.Processed(currentItem.cost*ItemShop.sellMultiplier));
        gameObject.SetActive(false);
        iss.Structure();
        currScale = initScale;
        scalableObj.transform.localScale = currScale;
        hovering = false;
        ii.UpdateInfo(null,currentItem);
    }

    public bool IsBeingSold(){
        return beingSold;
    }

    public bool DetectExistenceInShop(){
        if (PlayerInfo.GetIP().FindClassItemFromItem(currentItem) != null){
            gameObject.SetActive(true);
            return true;
        }
        gameObject.SetActive(false);
        return false;
    }
}
