using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPickups : MonoBehaviour
{
    [SerializeField] private float shieldAmountStep = -1f;
    [SerializeField] private float healthAmountStep = -1f;

    private void StepShield(){
        if (shieldAmountStep <= 0f) return;
        Pickup.StepShieldMod(shieldAmountStep);
    }

    private void StepHealth(){
        if (healthAmountStep <= 0f) return;
        Pickup.StepHealthMod(healthAmountStep);
    }

    private void Start(){
        ApplyEffect();
    }

    private void ApplyEffect(){
        StepShield();
        StepHealth();
    }

    private void Develop(){
        ApplyEffect();
    }
}
