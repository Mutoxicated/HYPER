using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSubscriber : MonoBehaviour
{
    [SerializeField] private ItemShop shop;
    [SerializeField] private int index;
    [HideInInspector] public Item currentItem;
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
        shop.subscribers.Add(UpdateItem);
        initScale = scalableObj.transform.localScale;
        maxScale = initScale*1.1f;
        currScale = initScale;
    }

    private void UpdateItem(){
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        currentItem = shop.currentItems[index];
        title.text = currentItem.itemName;
        title.color = currentItem.nameColor;
        image.sprite = currentItem.itemImage;
        cost.text = currentItem.cost.ToString()+"*";
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
    }

    public void ItemTaken(){
        if (PlayerInfo.GetMoney() < currentItem.cost)
            return;
        bool success = PlayerInfo.GetIP().AddItem(currentItem);
        if (!success)
            return;
        PlayerInfo.SetMoney(currentItem.cost);
        gameObject.SetActive(false);
        currScale = initScale;
        scalableObj.transform.localScale = currScale;
        hovering = false;
    }
}
