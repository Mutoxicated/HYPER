using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Recharge : MonoBehaviour
{
    [Header("World pieces")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private ScorePopupCanvas spc;
    [SerializeField] private TMP_Text percent;
    [Header("Mechanics")]
    [SerializeField] private OnInterval sphereScaleInterval;
    [SerializeField] private float duration;
    [SerializeField] private OnInterval durationInterval;
    [SerializeField] private OnInterval sphereDieInterval;
    [SerializeField] private int scoreToGive = 4000;

    private Vector3 initScale;
    private Vector3 alteredScale;
    private Material sphereMat;
    private Vector3 initSpcPos;
    private float yOff;
    private float time;

    private void OnTriggerEnter(){
        durationInterval.Resume();
    }
    private void OnTriggerStay(){
        durationInterval.Resume();
    }
    private void OnTriggerExit(){
        durationInterval.Pause();
    }

    private void OnEnable(){
        durationInterval.ChangeInterval(duration);
        float tx = Mathf.InverseLerp(0f,PlatformObjective.initPlatScale.x,transform.parent.parent.transform.localScale.x);
        alteredScale = transform.localScale;
        alteredScale.x *= tx;
        alteredScale.y *= tx;
        alteredScale.z *= tx;
        transform.localScale = alteredScale;
        
        initScale = transform.localScale;
        transform.localScale = Vector3.zero;

        transform.SetParent(transform,false);
        yOff = Mathf.Lerp(0f,15f,tx);
        transform.position = new Vector3(0f,yOff,0f);
        sphereMat = GetComponent<Renderer>().material;

        initSpcPos = spc.transform.position;
    }

    private void Update(){
        time += Time.deltaTime;
        if (time >= duration+50f){
            sphereDieInterval.enabled = true;
            spc.Die();
            return;
        }
        transform.localScale = Vector3.Lerp(transform.localScale,initScale,Time.deltaTime*15f);
        if (sphereDieInterval.enabled){
            percent.text = 100f+"%!";
            sphereMat.SetFloat("_Intact",sphereDieInterval.t);
        }else{  
            percent.text = Mathf.RoundToInt(durationInterval.t*100f).ToString()+"%";
        }
    }

    public void ChargedFull(){
        spc.transform.SetParent(null,false);
        spc.transform.position = new Vector3(transform.position.x,transform.position.y+yOff,transform.position.z);
        spc.transform.localRotation = Quaternion.identity;
        spc.transform.rotation = Quaternion.identity;
        if (!Difficulty.roundFinished){
            spc.PopScore(scoreToGive,4f,0f);
            PlayerInfo.AddScore(scoreToGive);
        }
        sphereDieInterval.enabled = true;
    }

    public void FinishObjective(){
        spc.Die();
        Destroy(gameObject.transform.parent.gameObject);
    }
}
