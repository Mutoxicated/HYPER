using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRandomly : MonoBehaviour
{
    [SerializeField] private int rotationInterval;
    [SerializeField, Range(0.2f,20f)] private float lerpSpeed;
    private Quaternion toRotation;
    private int time;

    private void Update()
    {
        time++;
        if (time >= rotationInterval)
        {
            toRotation = Random.rotation;
            time = 0;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime*lerpSpeed);
    }
}
