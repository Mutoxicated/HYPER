using UnityEngine;

public class SlotCollection : MonoBehaviour
{
    private SlotOccupant currentSelectedSO;

    public void SetCurrentSelectedSO(SlotOccupant so){
        currentSelectedSO = so;
    }

    public SlotOccupant GetCurrentSelectedSO(){
        return currentSelectedSO;
    }
}
