using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGradient : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private Gradient hitGradient;

    [SerializeField] private MaterialColorChannel colorChannel;
    [SerializeField] private int matIndex = 1;

    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().materials[matIndex];
    }

    private void Update()
    {
        mat.SetColor(colorChannel.ToString(), hitGradient.Evaluate(health.t));
    }
}
