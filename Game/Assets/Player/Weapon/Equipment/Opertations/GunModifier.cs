using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModifier : MonoBehaviour
{
    public bool focus;
    public float initialFocusIncrement;
    public float focusIncrement;

    private void OnEnable(){
        if (focus){
            PlayerInfo.GetGun().focus += focusIncrement;  
            PlayerInfo.GetGun().focus += initialFocusIncrement; 
        }
    }

    private void OnDisable(){
        if (focus){
            PlayerInfo.GetGun().focus -= focusIncrement;  
            PlayerInfo.GetGun().focus -= initialFocusIncrement;  
        }
    }
}
