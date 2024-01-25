using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRunData : MonoBehaviour
{
    [SerializeField] private GameObject objToDisable;
    [SerializeField] private GameObject objToEnable;

    private void Start(){
        if (RunDataSave.rData == null){
            objToDisable.SetActive(false);
            objToEnable.SetActive(true);
        }else{
            objToDisable.SetActive(true);
            objToEnable.SetActive(false);
        }
    }
}
