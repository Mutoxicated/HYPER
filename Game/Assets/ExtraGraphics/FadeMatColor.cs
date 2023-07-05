using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMatColor : MonoBehaviour
{
    [SerializeField] private string colorName = "_WireframeBackColour";
    [SerializeField] private int index = 1;
    [SerializeField] private Gradient gradient;
    [SerializeField] private bool useOnInterval = false;
    [SerializeField] private OnInterval onInterval;
    [SerializeField] private float interval;
    private float rate;
    private Material mat;
    private const float tau = Mathf.PI * 2f;
    private float angle = 0f;

    private float t;
    public Color color;

    void Start()
    {
        rate = Mathf.PI / (interval*Mathf.PI);
        var a = GetComponent<Renderer>().materials;
        mat = a[index];
    }

    void Update()
    {
        if (!useOnInterval)
        {
            angle += rate * Time.deltaTime;
            t = Mathf.Sin(angle);
            if (angle >= Mathf.PI)
            {
                angle = 0f;
            }
        }
        else if (useOnInterval)
        {
            t = onInterval.t;
        }
        color = gradient.Evaluate(t);
        mat.SetColor(colorName, color);
    }
}
