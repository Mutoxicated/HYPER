using System.Collections;
using System.Collections.Generic;
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
    private static float bombModifier = 1f;
    private static float shieldModifier = 1f;
    private static float healthModifier = 1f;

    private static float shieldAmount = 1;
    private static float healthAmount = 50;

    [SerializeField] private Collider col;
    [SerializeField] private PICKUP_TYPE pickup;
    [SerializeField] private float duration;
    [SerializeField] private OnInterval durationInterval;
    [SerializeField] private GameObject ps;
    [SerializeField] private OnInterval interval;
    [SerializeField] private Renderer[] rends;

    private List<Material> mats = new List<Material>();

    private Color color;

    public static void ResetMods(){
        shieldModifier = 1f;
        healthModifier = 1f;
        bombModifier = 1f;
    }

    public static void StepBombMod(float num){
        bombModifier += num;
    }

    public static void StepShieldMod(float num){
        shieldModifier += num;
    }

    public static void StepHealthMod(float num){
        healthModifier += num;
    }

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

    private int Processed(float value,float mod){
        return Mathf.RoundToInt(value*mod);
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
                PlayerInfo.GetGun().stats.GetHealth(Processed(healthAmount,healthModifier));
                PlayerInfo.GetPH().ActivateScreen(3);
                break;
            case PICKUP_TYPE.SHIELD:
                PlayerInfo.GetGun().stats.AddShield(Processed(shieldAmount,shieldModifier));
                PlayerInfo.GetPH().ActivateScreen(4);
                break;
            default:
                PlayerInfo.GetGun().stats.AddShield(Processed(shieldAmount,shieldModifier));
                break;
        }
    }

    public void DestroyPickup(){
        Destroy(transform.parent.gameObject);
    }
}
