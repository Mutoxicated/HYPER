using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoBox : MonoBehaviour
{
    public static InfoBox ib;

    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private float zOffset = -10f;
    private Vector3 alteredPos;

    public void PopBox(Vector3 pos, string title, string description){
        this.title.text = title;
        this.description.text ="Description: "+description;

        transform.parent.transform.position = pos;
        alteredPos = transform.parent.transform.localPosition;
        alteredPos.z = zOffset;
        transform.parent.transform.localPosition = alteredPos;

        transform.parent.gameObject.SetActive(true);
    }

    public void PopBox(Vector3 pos, Color titleColor, string title, string description){
        this.title.text = title;
        this.title.color = titleColor;
        this.description.text = "Description: "+description;

        transform.parent.transform.position = pos;
        alteredPos = transform.parent.transform.localPosition;
        alteredPos.z = zOffset;
        transform.parent.transform.localPosition = alteredPos;

        transform.parent.gameObject.SetActive(true);
    }

    public void UnpopBox(){
        transform.parent.gameObject.SetActive(false);
    }

    private void Awake(){
        ib = this;
        transform.parent.gameObject.SetActive(false);
    }
}
