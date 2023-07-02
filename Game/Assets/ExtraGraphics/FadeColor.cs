using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class FadeColor : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    private Material mat;
    private const float tau = Mathf.PI * 2f;
    private float angle = tau;
    [SerializeField, Range(tau / 10f, tau / 1800f)] private float rate;

    void Start()
    {
        var a = GetComponent<Renderer>().materials;
        mat = a[a.Length-1];
    }

    // Update is called once per frame
    void Update()
    {
        angle += rate * Time.deltaTime;
        float rawSineWave = Mathf.Sin(angle);

        mat.SetColor("_WireframeBackColour", gradient.Evaluate(Mathf.Abs(rawSineWave)));
    }
}
