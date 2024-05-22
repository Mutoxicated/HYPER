using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryDresser : MonoBehaviour
{
    [SerializeField] private List<Image> cellsOrdered = new List<Image>();

    [SerializeField] private Sprite fullCell;
    private Sprite emptyCell;

    private bool pendDecrease = false;
    private bool pending = false;
    private float time;
    private float interval = 0.5f;

    private int startIndex;
    private int endIndex;

    private void Start(){
        emptyCell = cellsOrdered[0].sprite;
    }

    private void Update(){
        if (!pending) return;
        time += Time.deltaTime;
        if (time >= interval){
            Switch();
            time = 0f;
        }
    }

    private void Switch(){
        for (int i = startIndex; i < endIndex; i++){
            if (cellsOrdered[i].sprite == emptyCell){
                cellsOrdered[i].sprite = fullCell;
            }else{
                cellsOrdered[i].sprite = emptyCell;
            }
        }
    }

    public void Increase(int index){
        if (pending) DeactivatePending();
        for (int i = index-1; i >= 0; i--){
            //Debug.Log("Batt"+i);
            cellsOrdered[i].sprite = fullCell;
        }
    }

    public void Decrease(int index){
        if (pending) DeactivatePending();
        for (int i = index; i < cellsOrdered.Count; i++){
            cellsOrdered[i].sprite = emptyCell;
        }
    }

    public void SetPending(int startIndex,int endIndex,bool pendDecrease){
        this.startIndex = startIndex;
        this.endIndex = endIndex;
        pending = true;
        this.pendDecrease = pendDecrease;
    }

    public void DeactivatePending(){
        pending = false;
        time = 0f;
        for (int i = startIndex; i < endIndex; i++){
            if (!pendDecrease)
                cellsOrdered[i].sprite = emptyCell;
            else
                cellsOrdered[i].sprite = fullCell;
        }
    }

    public bool GetPending(){
        return pending;
    }
}
