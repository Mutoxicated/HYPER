using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLerper : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private Transform transformToLerpTo;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private OnInterval interval;

    private float lerpSpeed;

    void Update()
    {
        lerpSpeed = curve.Evaluate(interval.t);
        _transform.position = Vector3.Lerp(_transform.position, transformToLerpTo.position, Time.deltaTime* lerpSpeed);
        _transform.rotation = Quaternion.Lerp(_transform.rotation, transformToLerpTo.rotation, Time.deltaTime* lerpSpeed*1.5f);
    }

    public void EnableTransformLerper(){
        interval.enabled = true;
        this.enabled = true;
    }

    public void SetTrans(Transform trans){
        _transform = trans;
    }

    public void SetTransToLerpTo(Transform trans){
        transformToLerpTo = trans;
    }

    public Transform GetTransToLerpTo(){
        return transformToLerpTo;
    }
}
