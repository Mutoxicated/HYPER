using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StaminaControl : MonoBehaviour
{
    [SerializeField] private staminaComms[] bars;
    
    public void ReduceStamina(float loss)
    {
        bars[0].LoseStamina(loss);
    }

    public float GetCurrentStamina()
    {
        float sum = 0f;
        foreach (var bar in bars)
        {
            sum += bar.GetStamina();
        }
        return sum;
    }
}
