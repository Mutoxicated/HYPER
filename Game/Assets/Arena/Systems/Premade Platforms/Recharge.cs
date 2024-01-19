using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Recharge : MonoBehaviour
{
    [Header("World pieces")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private TMP_Text percent;
    [Header("Mechanics")]
    [SerializeField] private OnInterval sphereScaleInterval;
    [SerializeField] private float duration;
    [SerializeField] private OnInterval durationInterval;
    [SerializeField] private OnInterval sphereDieInterval;
    [SerializeField] private float scoreToGive = 4000f;

    private Vector3 initScale;
    private Vector3 alteredScale;
    private Material sphereMat;

    private Vector3 initPlatScale = new Vector3(72f,2f,72f);

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
        float tx = Mathf.InverseLerp(0f,initPlatScale.x,transform.parent.parent.transform.localScale.x);
        alteredScale = transform.localScale;
        alteredScale.x *= tx;
        alteredScale.y *= tx;
        alteredScale.z *= tx;
        transform.localScale = alteredScale;
        
        initScale = transform.localScale;
        alteredScale = transform.localScale;
        alteredScale = initScale * sphereScaleInterval.t;
        transform.localScale = alteredScale;

        transform.SetParent(transform,false);
        transform.position = new Vector3(0f,Mathf.Lerp(0f,15f,tx),0f);
        sphereMat = GetComponent<Renderer>().material;
    }

    private void Update(){
        if (sphereScaleInterval.enabled){
            alteredScale = transform.localScale;
            alteredScale = initScale * sphereScaleInterval.t;
            transform.localScale = alteredScale;
        }
        if (sphereDieInterval.enabled){
            percent.text = 100f+"%!";
            sphereMat.SetFloat("_Intact",sphereDieInterval.t);
        }else{  
            percent.text = Mathf.RoundToInt(durationInterval.t*100f).ToString()+"%";
        }
    }

    public void ChargedFull(){
        sphereDieInterval.enabled = true;
    }

    public void FinishObjective(){
        Destroy(gameObject);
        Destroy(canvas);
        PlayerInfo.AddScore(scoreToGive);
    }
}
