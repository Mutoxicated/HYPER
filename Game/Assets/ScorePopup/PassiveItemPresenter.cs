using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveItemPresenter : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text title;

    public void SetImageSprite(Sprite sprite){
        image.sprite = sprite;
    }

    public void SetTitle(string text){
        title.text = text;
    }
}
