using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    [SerializeField] private SimonSays ss;
    [SerializeField] private int ID; 
    [SerializeField] private Renderer rend;
    [SerializeField, ColorUsage(true,true)] private Color dim;
    [SerializeField, ColorUsage(true,true)] private Color lit;

    private bool isLit = false;
    private float litTime = 0.5f;
    private float time;

    private Color alteredColor;

    public void LightUp(){
        ss.Pause();
        rend.materials[1].color = lit;
        isLit = true;
    }

    private void Start(){
        rend.materials[1].color = dim;
    }

    private void Update(){
        if (ss.dieInterval.enabled){
            rend.materials[1].SetFloat("_Intact",ss.dieInterval.t);
            return;
        }
        if (!isLit)
            return;
        time += Time.deltaTime;
        if (time >= litTime){
            time = 0f;
            rend.materials[1].color = dim;
            ss.Resume();
            isLit = false;
        }
    }

    public int GetID(){
        return ID;
    }

    private void OnTriggerEnter(Collider coll){
        if (!coll.isTrigger) return;
        if (coll.gameObject.tag != "Player") return;
        if (!ss.IsPlayersTurn()) return;
        
        ss.AddPlayerStep(ID);
        LightUp();
    }
}
