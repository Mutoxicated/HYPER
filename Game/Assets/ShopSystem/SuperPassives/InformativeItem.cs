using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InformativeItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text description;
    private PassiveItemInfo pii;

    public void SetPii(PassiveItemInfo pii){
        this.pii = pii;
        itemName.text = pii.itemName;
        itemName.color = pii.nameColor;
    }

    public PassiveItemInfo GetPii(){
        return pii;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.text = pii.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.text = "";
    }
}
