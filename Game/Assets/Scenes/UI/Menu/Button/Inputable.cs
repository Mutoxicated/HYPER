using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inputable : MonoBehaviour
{
    [SerializeField] private UnityEvent onInput = new UnityEvent();

    private Actions actions;

    private void Start(){
        actions = new Actions();
    }
}
