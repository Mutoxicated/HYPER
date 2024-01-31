using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveItemPresenter : MonoBehaviour
{
    [SerializeField] private PassiveItem currentPassive;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text title;

    public void SetImageSprite(Sprite sprite){
        image.sprite = sprite;
    }

    public void SetCurrentPassive(PassiveItem pi){
        currentPassive = pi;
    }

    public PassiveItem GetCurrentPassive(){
        return currentPassive;
    }

    public void SetTitle(string text,Color color){
        title.text = text;
        title.color = color;
    }
}
