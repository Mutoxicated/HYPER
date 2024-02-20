using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBoxPopper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Item optionalInfo;

    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoBox.ib.PopBox(transform.position,optionalInfo.nameColor,optionalInfo.itemName,optionalInfo.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoBox.ib.UnpopBox();
    }
}
