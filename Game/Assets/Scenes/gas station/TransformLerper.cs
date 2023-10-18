using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLerper : MonoBehaviour
{
    [SerializeField] private Transform transformToLerpTo;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private OnInterval interval;

    private float lerpSpeed;

    void Update()
    {
        lerpSpeed = curve.Evaluate(interval.t);
        transform.position = Vector3.Lerp(transform.position, transformToLerpTo.position, Time.deltaTime* lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, transformToLerpTo.rotation, Time.deltaTime* lerpSpeed*1.5f);
    }
}
