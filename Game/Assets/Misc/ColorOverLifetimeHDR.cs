using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOverLifetimeHDR : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField, GradientUsage(true)] private Gradient gradient;

    private Material mat;
    private float maxTime;
    private float t;

    private void Awake()
    {
        mat = particle.GetComponent<Renderer>().material;
        var main = particle.main;
        maxTime = main.duration;
    }

    private void Update()
    {
        if (!particle.isPlaying)
            return;
        if (particle.time != 0f)
        {
            t = particle.time / maxTime;
        }
        else
        {
            t = 0f;
        }
        mat.color = gradient.Evaluate(t);
    }

}
