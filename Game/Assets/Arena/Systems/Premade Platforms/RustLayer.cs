using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustLayer : MonoBehaviour, IDamageable
{
    [SerializeField] private Derust dr;

    [SerializeField] private int HP = 5;
    [SerializeField] private float steps = 4;

    private float currentHP = 6;
    private float currentStep = 0;
    private float t;

    private Vector3 alteredScale;
    private Vector3 initScale;

    private void Awake(){
        currentHP = HP;
        currentStep = 0;

        float tx = Mathf.InverseLerp(0f,PlatformObjective.initPlatScale.x,dr.holder.transform.parent.transform.localScale.x);
        float ty = Mathf.InverseLerp(0f,PlatformObjective.initPlatScale.y,dr.holder.transform.parent.transform.localScale.y);
        float tz = Mathf.InverseLerp(0f,PlatformObjective.initPlatScale.z,dr.holder.transform.parent.transform.localScale.z);
        alteredScale = transform.localScale;
        alteredScale.x *= tx;
        alteredScale.y *= ty*2f;
        alteredScale.z *= tz;
        transform.localScale = alteredScale;
        initScale = transform.localScale;
        EvaluateSize();
    }

    private void EvaluateSize(){
        t = Mathf.InverseLerp(steps,0,currentStep);
        transform.localScale = initScale * (1f+t/4f);
    }

    private float EvaluateDamage(Stats sender){
        if (sender.gameObject.tag != "bullets") return 0f;
        currentHP--;
        if (currentHP <= 0){
            currentStep++;
            EvaluateSize();
            currentHP = HP;
            if (currentStep >= steps){
                dr.EndObjective();
            }
        }
        return 1f;
    }

    public void TakeHealth(float intake, int shieldIntake)
    {
        //nun
    }

    public float TakeDamage(float intake, Stats sender, ref int shieldOut, float arbParam, int arbParam2)
    {
        Debug.Log(sender.gameObject.tag);
        if (sender.gameObject.tag != "bullets") return 0f;
        return EvaluateDamage(sender);
    }

    public float TakeDamage(float intake, Stats sender, float arbParam, int arbParam2)
    {
        Debug.Log(sender.gameObject.tag);
        if (sender.gameObject.tag != "bullets") return 0f;
        return EvaluateDamage(sender);
    }

    public float TakeDamage(float intake, float arbParam, int arbParam2)
    {
        return 0f;
        //nun
    }

    public void TakeInjector(Injector injector, bool cacheInstances)
    {
        //nun
    }

    public void RevertInjector(Injector refInjector)
    {
        //nun
    }
}
