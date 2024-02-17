using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inputable : MonoBehaviour
{
    [SerializeField] private PlayerInputContext pic;
    [SerializeField] private InputAction ia;
    [SerializeField] private Button optionalButton;
    [SerializeField] private UnityEvent onInput = new UnityEvent();

    private void OnEnable(){
        ia.Enable();
    }

    private void OnDisable(){
        ia.Disable();
    }

    private void Update(){
        if (pic.PauseMenuActive()) return;
        if (!gameObject.activeInHierarchy) return;
        if (ia.WasPressedThisFrame()){
            if (optionalButton != null){
                optionalButton.onClick.Invoke();
            }else{
                onInput.Invoke();
            }
        }
    }
}
