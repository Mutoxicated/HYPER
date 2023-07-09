using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGradient : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private Gradient hitGradient;

    [SerializeField] private string matColorName = "_WireframeBackColour";
    [SerializeField] private int matIndex = 1;

    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().materials[matIndex];
    }

    private void Update()
    {
        mat.SetColor(matColorName, hitGradient.Evaluate(health.t));
    }
}
