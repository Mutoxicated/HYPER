using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillUIBar : MonoBehaviour
{
    [SerializeField] private OnInterval interval;
    [SerializeField] private Image image;

    private void Update()
    {
        if (image.fillAmount != interval.t)
            image.fillAmount = interval.t;
    }
}
