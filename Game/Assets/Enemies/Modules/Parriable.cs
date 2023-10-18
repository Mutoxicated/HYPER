using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Parriable : MonoBehaviour, IParriable
{
    public bool parry = true;
    public UnityEvent<GameObject,Vector3> onParry = new UnityEvent<GameObject,Vector3>();

    public void ParrySetActive(bool state){
        parry = state;
    }

    public bool Parry(GameObject go, Vector3 position){
        if (parry){
            onParry.Invoke(go,position);
            Difficulty.utils.StopTime();
            return true;
        }else{
            return false;
        }
    }
}
