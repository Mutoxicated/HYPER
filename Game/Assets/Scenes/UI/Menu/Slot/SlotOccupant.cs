using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotOccupant : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PassiveItemPresenter pip;
    [SerializeField] private Slot slot;
    [SerializeField] private SlotCollection sc;
    private Slot newSlot;
    private SlotOccupant newSlotsSO;
    
    private bool dragged = false;
    private bool newSlotFound = false;

    private Vector3 slotPos;
    private Vector3 alteredPos;

    public void SetDragState(bool state){
        dragged = state;
        if (state){
            sc.SetCurrentSelectedSO(this);
        }else{
            sc.SetCurrentSelectedSO(null);
        }
    }

    public void GetNotified(Slot slot, SlotOccupant so){
        newSlot = slot;
        newSlotsSO = so;
        if (newSlot == null){
            newSlotFound = false;
            return;
        }
        newSlotFound = true;
    }

    public void UpdateSlot(Slot slot){
        alteredPos = slot.transform.position;
        alteredPos.x = transform.position.x;
        slotPos = alteredPos;
        transform.position = slotPos;
        slot.SetSO(this);
        this.slot = slot;
    }

    public Slot GetSlot(){
        return slot;
    }

    private void Start(){
        slotPos = transform.position;
    }

    private void UpdatePos(Vector3 wPos){
        alteredPos = wPos;
        alteredPos.x = transform.position.x;
        transform.position = alteredPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetDragState(true);
        UpdatePos(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdatePos(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!newSlotFound){
            transform.position = slotPos;
        }else if (newSlotFound){

            slot.SetSO(null);
            if (newSlotsSO != null){
                newSlotsSO.UpdateSlot(slot);
            }
            slot = newSlot;
            alteredPos = slot.transform.position;
            alteredPos.x = transform.position.x;
            slotPos = alteredPos;
            transform.position = slotPos;
            slot.SetSO(this);
            newSlotFound = false;
            newSlot = null;
            newSlotsSO = null;
        }
        SetDragState(false);
    }
}
