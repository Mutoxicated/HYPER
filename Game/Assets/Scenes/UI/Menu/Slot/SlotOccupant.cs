using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotOccupant : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
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
        newSlotFound = true;
    }

    public Slot Swap(Slot slot){
        return this.slot;
    }

    public void UpdateSlot(Slot slot){
        alteredPos = slot.transform.position;
        alteredPos.x = transform.position.x;
        slotPos = alteredPos;
        slot.SetSO(this);
        this.slot = slot;
    }

    public Slot GetSlot(){
        return slot;
    }

    private void GoToSlot(){
        
    }

    private void Start(){
        slotPos = transform.position;
    }

    private void Update(){
        if (!dragged && !newSlotFound){
            transform.position = slotPos;
        }else if (!dragged && newSlotFound){

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
        }
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
        SetDragState(false);
        UpdatePos(eventData.pointerCurrentRaycast.worldPosition);
    }
}
