using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineWidth : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private OnInterval interval;

    [SerializeField] private float multiplierMax;
    [SerializeField] private float multiplierMin;
    
    private void Update()
    {
        line.widthMultiplier = Mathf.Clamp01(Mathf.Lerp(multiplierMin, multiplierMax, interval.t));
    }

}
