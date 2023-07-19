using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMatColor : MonoBehaviour
{
    [SerializeField] private GameObject objectToCopy;
    [SerializeField] private int index = 0;
    [SerializeField] private ParticleSystem particle;

    private Material instance;

    private void Start()
    {
    }

    private void OnEnable()
    {
        instance = objectToCopy.GetComponent<Renderer>().materials[index];
        var main = particle.main;
        main.startColor = instance.color;
    }
}
