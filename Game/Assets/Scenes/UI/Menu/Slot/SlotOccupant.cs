using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SlotOccupant : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public PassiveItemPresenter pip;
    [SerializeField] private Slot slot;
    [SerializeField] private SlotCollection sc;
    private Slot newSlot;
    private SlotOccupant newSlotsSO;
    
    private bool dragged = false;
    private bool newSlotFound = false;
    private bool die = false;

    private Vector3 slotPos;
    private Vector3 alteredPos;
    private List<Slot> slotsToIgnore = new List<Slot>();

    private bool updatePos = false;
    private Vector3 mousePos;

    public void SetDragState(bool state){
        dragged = state;
        if (state){
            sc.SetCurrentSelectedSO(this);
        }else{
            sc.SetCurrentSelectedSO(null);
        }
    }

    public void SetIgnoredSlots(Slot[] slots){
        slotsToIgnore = slots.ToList();
    }

    private bool ValidSlot(Slot sl){
        if (slotsToIgnore == null) return true;
        foreach (Slot slot in slotsToIgnore){
            if (slot == null) continue;
            if (slot == sl){
                return false;
            }
        }
        return true;
    }

    public void GetNotified(Slot slot, SlotOccupant so){
        if (!ValidSlot(slot)) return;
        newSlot = slot;
        newSlotsSO = so;
        if (newSlot == null){
            newSlotFound = false;
            newSlotsSO = null;
            return;
        }
        newSlotFound = true;
    }

    public void GetNotifiedToDie(bool state){
        if (!ValidSlot(slot)) return;
        die = state;
    }

    public bool isValid(SlotOccupant so){
        if (slotsToIgnore.Count == so.slotsToIgnore.Count)
            return true;
        else return false;
    }

    public void UpdateSlot(Slot slot){
        alteredPos = slot.transform.position;
        alteredPos.x = transform.position.x;
        slotPos = alteredPos;
        transform.position = slotPos;
        slot.SetSO(this);
        transform.SetParent(slot.transform,true);
        this.slot = slot;
    }

    public Slot GetSlot(){
        return slot;
    }

    private void Start(){
        slotPos = transform.position;
    }

    private void Update(){
        if (!updatePos) return;
        mousePos = Mouse.current.position.ReadValue();
        mousePos.z = (transform.position.x-Camera.main.transform.position.z)*0.07f;
        UpdatePos(Camera.main.ScreenToWorldPoint(mousePos));
    }

    private void UpdatePos(Vector3 wPos){
        alteredPos = wPos;
        alteredPos.x = transform.position.x;
        transform.position = alteredPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetDragState(true);
        updatePos = true;
        //UpdatePos(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //UpdatePos(eventData.pointerCurrentRaycast.worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        updatePos = false;
        if (die){
            slot.SetSO(null);
            Destroy(gameObject);
        }
        if (!newSlotFound){
            transform.position = slotPos;
        }else if (newSlotFound){
            if (newSlotsSO != null){
                if (!newSlotsSO.isValid(this)){
                    newSlot = null;
                    newSlotsSO = null;
                    newSlotFound = false;
                    transform.position = slotPos;
                    return;
                }
                slot.SetSO(null);
                newSlotsSO.UpdateSlot(slot);
            }else{
                slot.SetSO(null);
            }
            slot = newSlot;
            alteredPos = slot.transform.position;
            alteredPos.x = transform.position.x;
            slotPos = alteredPos;
            transform.position = slotPos;
            slot.SetSO(this);
            transform.SetParent(slot.transform,true);
            newSlotFound = false;
            newSlot = null;
            newSlotsSO = null;
        }
        SetDragState(false);
    }
}
