using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ERotate : MonoBehaviour
{
    [SerializeField] private float rotationInterval;
    [SerializeField, Range(0.001f,20f)] private float lerpSpeed;
    [SerializeField] private Quaternion toRotation;
    [SerializeField] private bool randomRotation = true;
    private float time;

    private void Start()
    {
        if (randomRotation)
        {
            toRotation = Random.rotation;
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (rotationInterval < 0 || !randomRotation)
        {
            transform.Rotate(toRotation.eulerAngles*Time.deltaTime* lerpSpeed, Space.Self);
            return;
        }
        if (time >= rotationInterval)
        {
            toRotation = Random.rotation;
            time = 0;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime*lerpSpeed);
    }
}
