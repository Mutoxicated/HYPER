using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [SerializeField] private PlayerInputContext pic;
    [SerializeField] private Interacter interacter;
    [SerializeField] private GUIPopup popup;
    [SerializeField] private UnityEvent onInput = new UnityEvent();

    private bool rayHit;
    private bool once;

    private Actions actions;

    private void RayHitLogic(){
        if (interacter == null){
            rayHit = false;
            return;
        }
        if (interacter.GetGoHit() == gameObject) {
            rayHit = true;
            interacter.SetHittingInteractable(true);
        }else{
            rayHit = false;
        }
    }

    private void OnEnable(){
        actions = new Actions();
        actions.Abilities.Enable();
    }

    private void Update(){
        RayHitLogic();
        if (rayHit){
            once = true;
            popup.ActivatePopup();
            popup.AlterPopupText("Press ["+(actions.Abilities.LaunchOut_Interact.bindings[0].path[actions.Abilities.LaunchOut_Interact.bindings[0].path.Length-2]+"]").ToUpper());
            if (pic.WasPressedThisFrame("LaunchOut_Interact")){
                onInput.Invoke();
            }
        }else if (!rayHit && once) {
            once = false;
            popup.DeactivatePopup();
            interacter.SetHittingInteractable(false);
        }
    }
}
