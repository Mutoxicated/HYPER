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

    public void UpdateInfo(Item item) {
        title.text = item.itemName;
        title.color = item.nameColor;
        description.text = "Description: "+item.description;
    }

    public void PopBox(Vector2 pointerPos){
        transform.parent.transform.position = Camera.main.ScreenToWorldPoint(pointerPos);
        alteredPos = transform.parent.transform.localPosition;
        alteredPos.z = zOffset;
        transform.parent.transform.localPosition = alteredPos;

        transform.parent.gameObject.SetActive(true);
    }

    public void PopBox(Vector3 pos){
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
