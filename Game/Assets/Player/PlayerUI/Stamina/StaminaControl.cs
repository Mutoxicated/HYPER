using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class StaminaControl : MonoBehaviour
{
    [SerializeField] private TMP_Text sumText;
    [SerializeField] private staminaComms[] bars;

    private float currentSum;
    
    public void ReduceStamina(float loss)
    {
        bars[bars.Length-1].LoseStamina(loss);
    }

    private void Start()
    {
        currentSum = GetCurrentStamina();
        sumText.text = Mathf.Round(currentSum).ToString() + '%';
    }

    private void Update()
    {
        if (currentSum != GetCurrentStamina())
        {
            currentSum = Mathf.Lerp(currentSum, GetCurrentStamina(), Time.deltaTime*3f);
            sumText.text = Mathf.Round(currentSum).ToString()+'%';
        }
    }

    public float GetCurrentStamina()
    {
        float sum = 0f;
        foreach (var bar in bars)
        {
            if (bar.locked)
                break;
            sum += bar.GetStamina();
        }
        return sum;
    }
}
