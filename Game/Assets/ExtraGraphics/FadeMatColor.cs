using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMatColor : MonoBehaviour
{
    [SerializeField] private string colorName = "_WireframeBackColour";
    [SerializeField] private int index = 1;
    [SerializeField] private Gradient gradient;
    [SerializeField] private bool cutOff;
    private Material mat;
    private const float tau = Mathf.PI * 2f;
    private float angle = tau;
    [SerializeField, Range(tau / 10f, tau / 1800f)] private float rate;

    void Start()
    {
        var a = GetComponent<Renderer>().materials;
        mat = a[index];
    }

    void Update()
    {
        angle += rate * Time.deltaTime;
        float rawSineWave = Mathf.Sin(angle);

        if (cutOff && rawSineWave >= 0.98f)
        {
            angle = tau;
        }

        mat.SetColor(colorName, gradient.Evaluate(Mathf.Abs(rawSineWave)));
    }
}
