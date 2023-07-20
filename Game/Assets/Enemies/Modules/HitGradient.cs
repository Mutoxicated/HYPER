using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGradient : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField,GradientUsage(true)] private Gradient[] hitGradient;
    [SerializeField] private int index = 0;

    [SerializeField] private int matIndex = 1;

    private Material mat;
    [HideInInspector] public Color color;

    private void Awake()
    {
        color = hitGradient[index].Evaluate(health.t);
    }

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
        color = hitGradient[index].Evaluate(health.t);
        mat.color = hitGradient[index].Evaluate(health.t);
    }
}
