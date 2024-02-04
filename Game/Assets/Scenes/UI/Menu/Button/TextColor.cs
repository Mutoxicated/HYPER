using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextColor : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Image imageReceiver;
    [SerializeField] private Image image;
    
    private void Update(){
        if (text == null)
            imageReceiver.color = image.color;
        else
            text.color = image.color;
    }
}
