using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyFadeColor : MonoBehaviour
{
    [SerializeField] private FadeMatColor colorToCopy;
    [SerializeField] private ParticleSystem particle;

    private void OnEnable()
    {
        var customData = particle.customData;
        customData.SetColor(ParticleSystemCustomData.Custom1, colorToCopy.color);
    }
}
