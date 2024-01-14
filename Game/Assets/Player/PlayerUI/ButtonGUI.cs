using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ButtonGUI : MonoBehaviour
{
    [SerializeField] private string inputString;
    [SerializeField] private UnityEvent onInputDown = new UnityEvent();

    private ButtonInput input;
    private bool hovering;

    private Actions actions;

    private void Start(){//TODO: Fix static guipopup not working, find a way to only get the KeyCode of the action binding and migrate to inputSystem
        actions = new Actions();
        input = new ButtonInput(inputString);
        Debug.Log(actions.LaunchOut.Get().actions[0].bindings[0].path);
    }

    private void Update(){
        if (!hovering)
            return;
        if (input.GetInputDown()){
            onInputDown.Invoke();
        }
        GUIPopup.guiPopup.AlterPopupText("Press ["+actions.LaunchOut.Get().name+"]");
    }

    public void SetHovering(bool state){
        hovering = state;
        GUIPopup.guiPopup.AlterPopupText("Press ["+actions.LaunchOut.Get().name+"]");
        if (hovering){
            GUIPopup.guiPopup.ActivatePopup();
        }else{
            GUIPopup.guiPopup.DeactivatePopup();
        }
    }

}
