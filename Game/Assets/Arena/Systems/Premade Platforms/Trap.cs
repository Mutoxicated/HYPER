using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
[Header("World pieces")]
    [SerializeField] private ScorePopupCanvas spc;
    [Header("Mechanics")]
    [SerializeField] private float duration;
    [SerializeField] private OnInterval durationInterval;
    [SerializeField] private OnInterval sphereDieInterval;
    [SerializeField] private int scoreToGive = 1000;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private OnInterval damageOnInterval;

    private Vector3 initScale;
    private Vector3 alteredScale;
    private Material sphereMat;
    private Vector3 initSpcPos;
    private float yOff;

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
        if (Difficulty.roundFinished){
            Ended();
            return;
        }
        damageOnInterval.ChangeInterval(damageInterval);
        damageOnInterval.Pause();
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

        if (spc != null)
            initSpcPos = spc.transform.position;
    }

    private void Update(){
        transform.localScale = Vector3.Lerp(transform.localScale,initScale,Time.deltaTime*15f);
        if (sphereDieInterval.enabled){
            sphereMat.SetFloat("_Intact",sphereDieInterval.t);
            return;
        }
        float distance = Vector3.Distance(transform.position,PlayerInfo.GetPlayer().transform.position);
        if (distance > transform.lossyScale.magnitude*0.5f+7f && !damageOnInterval.isPlaying){
            damageOnInterval.Resume();
        }else if (distance <= transform.lossyScale.magnitude*0.5f+7f && damageOnInterval.isPlaying){
            damageOnInterval.Pause();
        }
    }

    public void DamagePlayer(){
        PlayerInfo.GetPH().TakeDamage(damage,1f,0);
    }

    public void Ended(){
        if (spc != null){
            spc.transform.SetParent(null,false);
            spc.transform.position = new Vector3(transform.position.x,transform.position.y+yOff,transform.position.z);
            spc.transform.localRotation = Quaternion.identity;
            spc.transform.rotation = Quaternion.identity;
            if (!Difficulty.roundFinished){
                spc.PopScore(scoreToGive,4f,0f);
                PlayerInfo.AddScore(scoreToGive);
            }
            spc.Die();
        }
        damageOnInterval.enabled = false;
        sphereDieInterval.enabled = true;
    }

    public void FinishObjective(){
        Destroy(gameObject.transform.parent.gameObject);
    }
}
