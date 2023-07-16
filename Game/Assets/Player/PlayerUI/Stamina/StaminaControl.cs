using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class StaminaControl : MonoBehaviour
{
    [SerializeField] private staminaComms[] bars;
    
    public void ReduceStamina(float loss)
    {
        bars[bars.Length-1].LoseStamina(loss);
    }

    public float GetCurrentStamina()
    {
        float sum = 0f;
        foreach (var bar in bars)
        {
            if (bar.enabled)
                sum += bar.GetStamina();
        }
        return sum;
    }
}
