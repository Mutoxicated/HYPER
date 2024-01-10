using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PICKUP_TYPE {
    SHIELD,
    HEALTH,
    POWERUP,
    ATOMIZE_BOMB,
    FREEZE_BOMB
}

public class Pickup : MonoBehaviour
{
    private static int shieldAmount = 1;
    private static int healthAmount = 50;

    [SerializeField] private Collider col;
    [SerializeField] private PICKUP_TYPE pickup;
    [SerializeField] private float duration;
    [SerializeField] private OnInterval durationInterval;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private GameObject ps;
    [SerializeField] private OnInterval interval;
    [SerializeField] private Renderer[] rends;

    private List<Material> mats = new List<Material>();

    private Color color;

    private void Start(){
        foreach (Renderer rend in rends){
            mats.Add(rend.material);
        }
        durationInterval.ChangeInterval(duration);
        durationInterval.enabled = true;
    }

    private void Update(){
        if (interval.enabled){
            foreach (Material mat in mats){
                mat.SetFloat("_Intact",interval.t);
            }
        }
    }

    private void LateUpdate(){
        if (durationInterval.enabled){
            foreach (Material mat in mats){
                color = mat.GetColor("_WireframeBackColour");
                color.a = Mathf.Abs(durationInterval.t-1);
                mat.SetColor("_WireframeBackColour",color);
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag != "Player")
            return;
        col.enabled = false;
        interval.enabled = true;
        durationInterval.enabled = false;
        ps.SetActive(true);
        ps.transform.SetParent(null);
        switch(pickup){
            case PICKUP_TYPE.HEALTH:
                PlayerInfo.GetGun().stats.GetHealth(healthAmount);
                PlayerInfo.GetPH().ActivateScreen(3);
                break;
            case PICKUP_TYPE.SHIELD:
                PlayerInfo.GetGun().stats.AddShield(shieldAmount);
                PlayerInfo.GetPH().ActivateScreen(4);
                break;
            default:
                PlayerInfo.GetGun().stats.AddShield(shieldAmount);
                break;
        }
    }

    public void DestroyPickup(){
        Destroy(transform.parent.gameObject);
    }
}
