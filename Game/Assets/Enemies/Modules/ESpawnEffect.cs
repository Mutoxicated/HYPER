using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESpawnEffect : MonoBehaviour
{
    [SerializeField] private int wireMatIndex;
    [SerializeField] private int baseMatIndex;
    private Material wireframe;
    private Material baseMat;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float accel;
    private float t;

    [ColorUsage(true,true)] private Color baseColor;

    private void ChangeBaseColor()
    {
        baseColor = baseMat.color;
        baseColor.a = t;
        baseMat.color = baseColor;
    }

    private void Awake()
    {
        if (wireMatIndex >= 0)
        {
            wireframe = GetComponent<Renderer>().materials[wireMatIndex];
            wireframe.SetFloat("_Intact", Mathf.Abs(t-1));
        }
        if (baseMatIndex >= 0)
        {
            baseMat = GetComponent<Renderer>().materials[baseMatIndex];
            ChangeBaseColor();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        t = Mathf.Lerp(t, 1f, Time.deltaTime * lerpSpeed);
        if (wireframe != null)
        {
            wireframe.SetFloat("_Intact", Mathf.Abs(t - 1));
            if (wireframe.GetFloat("_Intact") < 0.01)
            {
                wireframe.SetFloat("_Intact", 0);
                this.enabled = false;
            }
        }
        if (baseMat != null)
        {
            ChangeBaseColor();
            if (baseColor.a > 0.99f)
            {
                baseColor.a = 1f;
                baseMat.color = baseColor;
                this.enabled = false;
            }
        }
        lerpSpeed += accel;
    }
}
