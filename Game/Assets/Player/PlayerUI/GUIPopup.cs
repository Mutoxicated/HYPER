using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIPopup : MonoBehaviour
{
    public static GUIPopup guiPopup;
    [SerializeField] private TMP_Text popup;

    private void Start(){
        guiPopup = this;
    }

    public void ActivatePopup(){
        gameObject.SetActive(true);
    }

    public void AlterPopupText(string text){
        popup.text = text;
    }

    public void DeactivatePopup(){
        gameObject.SetActive(false);
    }
}
