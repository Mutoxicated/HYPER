using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPassive : MonoBehaviour
{
    [SerializeField,Range(0,16)] private int index;

    private bool develop = false;

    public int GetIndex(){
        return index;
    }

    public void SetDevelopState(bool state){
        develop = state;
    }

    public bool GetDevelopState(){
        return develop;
    }
}
