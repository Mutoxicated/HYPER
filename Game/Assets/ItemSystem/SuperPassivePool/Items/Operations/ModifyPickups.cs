using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPickups : MonoBehaviour
{
    [SerializeField] private SuperPassive sp;
    [SerializeField] private float shieldAmountStep = -1f;
    [SerializeField] private float healthAmountStep = -1f;
    [SerializeField] private float bombAmountStep = -1f;

    private void StepShield(int num){
        if (shieldAmountStep <= 0f) return;
        Pickup.StepShieldMod(shieldAmountStep*num);
    }

    private void StepHealth(int num){
        if (healthAmountStep <= 0f) return;
        Pickup.StepHealthMod(healthAmountStep*num);
    }

    private void StepBomb(int num){
        if (bombAmountStep <= 0f) return;
        Pickup.StepBombMod(bombAmountStep*num);
    }

    private void Start(){
        sp.subs.Add(ApplyEffect);
    }

    private void ApplyEffect(int num){
        StepBomb(num);
        StepShield(num);
        StepHealth(num);
    }
}
