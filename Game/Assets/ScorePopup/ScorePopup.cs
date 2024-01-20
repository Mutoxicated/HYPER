using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    [SerializeField] private RectTransform trans;
    [SerializeField] private TMP_Text txt;
    private string plus = "+";
    
    private float duration = 1f;
    private float yInc = 180f;
    private float time = 0f;
    private Vector3 alteredPos;
    private Color alteredCol;
    private float t;

    private void Update(){
        time += Time.deltaTime;
        if (time >= duration){
            gameObject.SetActive(false);
            time = 0f;
            trans.localPosition = Vector3.zero;
            ScorePopupPool.spp.ReturnObject(this);
            return;
        }
        alteredCol = txt.color;

        t = Mathf.InverseLerp(0f,duration,time);
        alteredCol.a = Mathf.Lerp(1f,0f,t);
        txt.color = alteredCol;
        
        alteredPos = trans.localPosition;
        alteredPos.y += yInc*Time.deltaTime;
        trans.localPosition = alteredPos;
    }    

    public void SetText(int score){
        txt.text = plus+score.ToString();
    }

    public void SetDuration(float duration){
        this.duration = duration;
    }
}
