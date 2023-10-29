using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    public PassivePool origin;
    public int population = 1;

    private void OnDisable(){
        population = 1;
    }
}
