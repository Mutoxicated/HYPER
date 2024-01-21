using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Interacter interacter;
    [SerializeField] private GUIPopup popup;
    [SerializeField] private UnityEvent onInput = new UnityEvent();

    private string popupText = "Press";
    private bool rayHit;
    private bool once;

    private Actions actions;

    private void Awake(){
        actions = new Actions();
    }

    private void RayHitLogic(){
        if (interacter == null){
            rayHit = false;
            return;
        }
        if (interacter.GetGoHit() == gameObject) {
            rayHit = true;
        }else{
            rayHit = false;
        }

    }

    private void OnEnable(){
        actions.Interact.Enable();
    }

    private void Update(){
        RayHitLogic();
        if (rayHit){
            once = true;
            popup.ActivatePopup();
            popup.AlterPopupText("Press ["+(actions.Interact.interact.bindings[0].path[actions.Interact.interact.bindings[0].path.Length-1]+"]").ToUpper());
            if (actions.Interact.interact.WasPerformedThisFrame()){
                onInput.Invoke();
            }
        }else if (!rayHit && once) {
            once = false;
            popup.DeactivatePopup();
        }
    }
}
