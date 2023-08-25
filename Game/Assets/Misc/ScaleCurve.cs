using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleCurve : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private AnimationCurve curveX;
    [SerializeField] private AnimationCurve curveY;
    [SerializeField] private AnimationCurve curveZ;
    [SerializeField] private OnInterval interval;

    private Vector3 initialScale;
    private Vector3 scale = Vector3.one;

    private void CalculateScale()
    {
        scale.x = initialScale.x * curveX.Evaluate(interval.t);
        scale.y = initialScale.y * curveY.Evaluate(interval.t);
        scale.z = initialScale.z * curveZ.Evaluate(interval.t);
        transform.localScale = scale;
    }

    private void Awake()
    {
        initialScale = transform.localScale;
        CalculateScale();
    }

    private void Update()
    {
        CalculateScale();
    }
}
