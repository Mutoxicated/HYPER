using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyFadeColor : MonoBehaviour
{
    [SerializeField] private FadeMatColor colorToCopy;
    [SerializeField] private ParticleSystem particle;

    private void OnEnable()
    {
        var main = particle.main;
        main.startColor = colorToCopy.color;
    }
}
