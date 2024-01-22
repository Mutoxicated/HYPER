using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePopup : MonoBehaviour
{
    private static readonly float initYInc = 300f;
    private static readonly float initDuration = 1f;

    [SerializeField] private RectTransform trans;
    [SerializeField] private TMP_Text txt;
    private string plus = "+";
    
    private float duration = 1f;
    private float yInc = initYInc;
    private float time = 0f;
    private Vector3 alteredPos;
    private Color alteredCol;
    private float t;

    private void OnDisable(){
        yInc = initYInc;
        duration = initDuration;
        time = 0f;
    }

    private void Update(){
        time += Time.deltaTime;
        if (time >= duration){
            gameObject.SetActive(false);
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

    public void SetYInc(float yInc){
        this.yInc = yInc;
    }
}
