using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGradient : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField,GradientUsage(true)] private Gradient[] hitGradient;
    [SerializeField] private int index = 0;

    [SerializeField] private MaterialColorChannel colorChannel;
    [SerializeField] private int matIndex = 1;

    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().materials[matIndex];
    }

    public void ChangeColorIndex(int i)
    {
        index = i;
    }

    private void Update()
    {
        mat.SetColor(colorChannel.ToString(), hitGradient[index].Evaluate(health.t));
    }
}
