using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputContext : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Interacter interacter;

    private Actions actions;

    public bool PauseMenuActive(){
        return pauseMenu.activeSelf;
    }

    public bool WasPressedThisFrame(string name){
        if (pauseMenu.activeSelf) return false;
        return actions.FindAction(name).WasPressedThisFrame();
    }

    public bool WasPerformedThisFrame(string name){
        if (pauseMenu.activeSelf) return false;
        return actions.FindAction(name).WasPerformedThisFrame();
    }

    public bool IsPressed(string name){
        if (pauseMenu.activeSelf) return false;
        return actions.FindAction(name).IsPressed();
    }

    public float GetHorizontal(){
        return actions.WASD.ad.ReadValue<float>();
    }

    public float GetVertical(){
        return actions.WASD.ws.ReadValue<float>();
    }

    public bool LaunchOutWasPressed(){
        if (interacter != null){
            if (interacter.GetHittingInteractable()) return false;
        }
        return actions.Abilities.LaunchOut_Interact.WasPressedThisFrame();
    }

    private void OnEnable(){
        actions = new Actions();
        actions.Enable();
    }
}
