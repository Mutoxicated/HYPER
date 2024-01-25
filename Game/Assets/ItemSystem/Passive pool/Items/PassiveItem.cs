using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum PassiveOperators{
    ADD,
    SUBTRACT,
    MULTIPLY,
    DIVIDE,
    MODULO,
    POWER
}

public class PassiveItem : MonoBehaviour
{

    public PassivePool origin;
    public int population = 1;

    public float number;
    public PassiveOperators _operator;

    private void OnDisable(){
        population = 1;
    }
}
