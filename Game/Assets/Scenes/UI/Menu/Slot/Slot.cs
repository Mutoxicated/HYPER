using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private SlotCollection sc;
    [SerializeField] private SlotOccupant so;

    [SerializeField,Range(0.01f,5f)] private float radius;

    private bool once = false;

    public void SetSO(SlotOccupant so){
        this.so = so;
    }

    public SlotOccupant GetSO(){
        return so;
    }

    private void Update(){
        if (sc.GetCurrentSelectedSO() == null) return;

        if (sc.GetCurrentSelectedSO().GetSlot() == this) return;

        if (Vector3.Distance(sc.GetCurrentSelectedSO().transform.position,transform.position) > radius && !once){
            sc.GetCurrentSelectedSO().GetNotified(null,null);
            once = true;
        }else if (Vector3.Distance(sc.GetCurrentSelectedSO().transform.position,transform.position) <= radius && once){
            once = false;
            sc.GetCurrentSelectedSO().GetNotified(this,so);
        }
    }
}
